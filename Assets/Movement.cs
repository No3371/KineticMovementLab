using BAStudio.StatePattern;
using UnityEngine;

namespace BAStudio.Unity.KineticMovementLab
{
    public class Movement : MonoBehaviour
    {
        StateMachine<Movement> stateMachine;
        [SerializeField]
        int ID;
        public CharacterController characterController;
        public float maxSpeed, acceleration, autoDeceleration, jumpingPower;
        Vector3 velocity;
        CollisionFlags collisionFlags;

        void Start ()
        {
            stateMachine = new StateMachine<Movement>(this);
            stateMachine.ChangeState<Idle>();
        }
        
        void Update ()
        {
            stateMachine.Update();
        }

        void MoveWithCharacterController ()
        {
            Debug.Log(velocity * Time.deltaTime);
            collisionFlags = characterController.Move(velocity * Time.deltaTime);
            if ((collisionFlags & CollisionFlags.CollidedBelow) > 0)
            {
                velocity.y = 0;
            }
        }

        void ApplyGravity ()
        {
            velocity += Physics.gravity * Time.deltaTime;
        }

        class Idle : State<Movement>
        {
            public override void OnEntered(StateMachine<Movement> machine, State<Movement> previous) {}

            public override void OnLeaving(StateMachine<Movement> machine, State<Movement> next) {}

            public override void Update(StateMachine<Movement> machine)
            {
                
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    machine.ChangeState<Jumping>();
                    return;
                }
                if (Input.GetKeyDown(KeyCode.W))
                {
                    machine.ChangeState<Moving>();
                    machine.Update(); // Don't wait until next frame to start moving
                    return;
                }
                machine.Target.ApplyGravity();
                machine.Target.MoveWithCharacterController();
            }
        }

        class Falling : State<Movement>
        {
            public override void OnEntered(StateMachine<Movement> machine, State<Movement> previous)
            {
                throw new System.NotImplementedException();
            }

            public override void OnLeaving(StateMachine<Movement> machine, State<Movement> next)
            {
                throw new System.NotImplementedException();
            }

            public override void Update(StateMachine<Movement> machine)
            {
                machine.Target.ApplyGravity();
                machine.Target.MoveWithCharacterController();
            }
        }

        class Moving : State<Movement>
        {
            public override void OnEntered(StateMachine<Movement> machine, State<Movement> previous)
            {
            }

            public override void OnLeaving(StateMachine<Movement> machine, State<Movement> next)
            {
            }

            public override void Update(StateMachine<Movement> machine)
            {
                var dir = machine.Target.velocity.normalized;
                var currentSpeed = machine.Target.velocity.sqrMagnitude;
                if (Input.GetKey(KeyCode.W))
                {
                    machine.Target.velocity = dir * (machine.Target.acceleration * Time.deltaTime + currentSpeed);
                }
                else
                {
                    currentSpeed -= machine.Target.autoDeceleration * Time.deltaTime;
                    if (currentSpeed <= 0)
                    {
                        currentSpeed = 0;
                        machine.ChangeState<Idle>();
                    }
                    machine.Target.velocity = dir * currentSpeed;
                }
                machine.Target.ApplyGravity();
                machine.Target.MoveWithCharacterController();
            }
        }

        class Jumping : State<Movement>
        {
            public override void OnEntered(StateMachine<Movement> machine, State<Movement> previous)
            {
                machine.Target.velocity += Vector3.up * machine.Target.jumpingPower * Time.deltaTime;
            }

            public override void OnLeaving(StateMachine<Movement> machine, State<Movement> next)
            {
            }

            public override void Update(StateMachine<Movement> machine)
            {
                machine.Target.ApplyGravity();
                machine.Target.MoveWithCharacterController();
            }
        }
    }

}
