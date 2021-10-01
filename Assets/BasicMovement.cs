using System;
using System.Collections;
using BAStudio.StatePattern;
using MiscUtil;
using Unity.VisualScripting;
using UnityEngine;

namespace BAStudio.Unity.KineticMovementLab
{

    public class BasicMovement : MovementBase<Record>
    {
        StateMachine<BasicMovement> stateMachine;
        [SerializeField]
        public CharacterController characterController;
        public float maxSpeed, acceleration, autoDeceleration, jumpingPower;
        Vector3 velocity;
        RaycastCollider raycastCollider;
        InputSignal InputPreviousFrame { get; set; }
        InputSignal ActiveInput { get; set; }

        void Start ()
        {
            stateMachine = new StateMachine<BasicMovement>(this);
            stateMachine.DebugOutput += Debug.Log;
            stateMachine.ChangeState<GroundedIdle>();
        }
        
        void Update ()
        {
            InputPreviousFrame = ActiveInput;
            ActiveInput = new InputSignal {
                Jump = Input.GetKeyUp(KeyCode.Space),
                Vertical = (Input.GetKey(KeyCode.W)? 1 : 0) + (Input.GetKey(KeyCode.S)? -1 : 0),
                Horizontal = (Input.GetKey(KeyCode.A)? -1 : 0) + (Input.GetKey(KeyCode.D)? 1 : 0),
            };
            stateMachine.Update();
        }

        void Rotate (Vector3 euler)
        {
            this.transform.Rotate(euler, Space.Self);
        }

        CollisionEvent Move ()
        {
            if (raycastCollider.Cast(velocity * Time.deltaTime))
            this.transform.Translate();
        }

        void ApplyGravity ()
        {
            velocity += Physics.gravity * Time.deltaTime;
        }

        public override bool ShouldRecord()
        {
            return ActiveInput == InputPreviousFrame;
        }

        public override Record Record()
        {
            return new Record () {
                Position = this.transform.position,
                Rotation = this.transform.rotation,
            };
        }

        struct InputSignal : IEquatable<InputSignal>
        {
            public float Horizontal { get; set; }
            public float Vertical   { get; set; }
            public bool  Jump       { get; set; }
            
            public override bool Equals(object obj)
            {
                return obj is InputSignal other && Equals(other);
            }

            public bool Equals(InputSignal other)
            {
                return this.Horizontal == other.Horizontal
                    && this.Vertical   == other.Vertical
                    && this.Jump       == other.Jump;
            }

            public override int GetHashCode()
            {
                unchecked // Overflow is fine, just wrap
                {
                    int hash = 571;
                    hash = hash * 28657 + Horizontal.GetHashCode();
                    hash = hash * 28657 + Vertical.GetHashCode();
                    hash = hash * 28657 + Jump.GetHashCode();
                    return hash;
                }
            }

            public static bool operator ==(InputSignal subject, InputSignal other) => subject.Equals(other);
            public static bool operator !=(InputSignal subject, InputSignal other) => !(subject == other);
        }

        class GroundedIdle : State<BasicMovement>
        {
            public override void OnEntered(StateMachine<BasicMovement> machine, State<BasicMovement> previous, BasicMovement context)
            {
                if (context.velocity.x != 0 || context.velocity.z != 0) machine.ChangeState<GroundedMoving>();
            }

            public override void OnLeaving(StateMachine<BasicMovement> machine, State<BasicMovement> next, BasicMovement context) {}

            public override void Update(StateMachine<BasicMovement> machine, BasicMovement context)
            {
                if (context.ActiveInput.Jump)
                {
                    machine.ChangeState<Jumping>();
                    machine.Update();
                    return;
                }

                if (context.ActiveInput.Vertical != 0 || context.ActiveInput.Horizontal != 0)
                {
                    machine.ChangeState<GroundedMoving>();
                    machine.Update(); // Don't wait until next frame to start moving
                    return;
                }

                context.ApplyGravity();
                if ((context.Move() & CollisionFlags.CollidedBelow) == 0)
                {
                    machine.ChangeState<Falling>();
                    return;
                }

                context.velocity.y = 0;
            }
        }

        class Falling : State<BasicMovement>
        {
            public override void OnEntered(StateMachine<BasicMovement> machine, State<BasicMovement> previous, BasicMovement context)
            {
            }

            public override void OnLeaving(StateMachine<BasicMovement> machine, State<BasicMovement> next, BasicMovement context)
            {
            }

            public override void Update(StateMachine<BasicMovement> machine, BasicMovement context)
            {
                context.ApplyGravity();
                if ((context.Move() & CollisionFlags.CollidedBelow) > 0)
                {
                    machine.ChangeState<GroundedIdle>();
                    return;
                }
            }
        }

        class GroundedMoving : State<BasicMovement>
        {
            public override void OnEntered(StateMachine<BasicMovement> machine, State<BasicMovement> previous, BasicMovement context)
            {
            }

            public override void OnLeaving(StateMachine<BasicMovement> machine, State<BasicMovement> next, BasicMovement context)
            {
            }

            public override void Update(StateMachine<BasicMovement> machine, BasicMovement context)
            {
                context.velocity += context.ActiveInput.Vertical * context.transform.forward * context.acceleration * Time.deltaTime;
                context.velocity += context.ActiveInput.Horizontal * context.transform.right * context.acceleration * Time.deltaTime;

                if (context.ActiveInput.Jump)
                {
                    machine.ChangeState<Jumping>();
                    machine.Update();
                    return;
                }

                if (context.ActiveInput.Vertical == 0 && context.ActiveInput.Horizontal == 0 && context.InputPreviousFrame.Vertical == 0 && context.InputPreviousFrame.Horizontal == 0)
                {
                    var dir = context.velocity.normalized;
                    var currentSpeed = context.velocity.magnitude;
                    currentSpeed -= context.autoDeceleration * Time.deltaTime;
                    if (currentSpeed <= 0)
                    {
                        currentSpeed = 0;
                        machine.ChangeState<GroundedIdle>();
                    }
                    context.velocity = dir * currentSpeed;
                }


                context.ApplyGravity();
                if ((context.Move() & CollisionFlags.CollidedBelow) == 0)
                {
                    machine.ChangeState<Falling>();
                    return;
                }

                context.velocity.y = 0;
            }
        }

        class Jumping : State<BasicMovement>
        {
            public override void OnEntered(StateMachine<BasicMovement> machine, State<BasicMovement> previous, BasicMovement context)
            {
                context.velocity += Vector3.up * context.jumpingPower;
            }

            public override void OnLeaving(StateMachine<BasicMovement> machine, State<BasicMovement> next, BasicMovement context)
            {
            }

            public override void Update(StateMachine<BasicMovement> machine, BasicMovement context)
            {
                context.ApplyGravity();
                if (context.velocity.y <= 0)
                {
                    machine.ChangeState<Falling>();
                    return;
                }
                
                if ((context.Move() & CollisionFlags.CollidedBelow) > 0)
                {
                    machine.ChangeState<GroundedIdle>();
                    return;
                }
            }
        }
    }

}
