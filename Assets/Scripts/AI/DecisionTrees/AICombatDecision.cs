using UnityEngine;

namespace AI.DecisionTrees
{
    public class AICombatDecision
    {
        private int attackDelayTimer;
        private readonly DummyPlayer _player;
        private readonly GameObject _ai;
        private readonly Animator _anim;
        public bool AttackingTag { get; private set; }
        public IDecisionTreeNode AttackingNodes { get; }

        public AICombatDecision(DummyPlayer player, GameObject ai, Animator anim)
        {
            _player = player;
            _ai = ai;
            _anim = anim;

            attackDelayTimer = 100;

            IDecisionTreeNode circlePlayer = new ActionNode(CirclePlayer);
            IDecisionTreeNode attackPlayer = new ActionNode(Attack);
            IDecisionTreeNode getToPlayer = new ActionNode(GetPlayerPosition);

            IDecisionTreeNode canAttack = new DecisionNode(GetPlayerIsNear, attackPlayer, getToPlayer);
            AttackingNodes = new DecisionNode(HasSpaceNearPlayer, canAttack, circlePlayer);
        }

        public void PlayerIsInArea()
        {
            _anim.SetTrigger("Cast Spell");
        }

        private bool HasSpaceNearPlayer()
            => _player.NOfGhostsAround < 4 || AttackingTag;

        private bool GetPlayerIsNear()
        {
            float distanceToPlayer = Vector3.Distance(
                _ai.transform.position, _player.transform.position);

            if (!(distanceToPlayer <= 2f) && AttackingTag)
            {
                _player.NOfGhostsAround -= 1;
                AttackingTag = false;
            }

            return distanceToPlayer <= 2f;
        }

        private Vector3 Attack()
        {
            attackDelayTimer++;

            Vector3 dir = _ai.transform.position - _player.transform.position;
            // Resets the value of Y to 0
            dir.y = 0;

            // Rotates gradually the Ghost towards the direction
            _ai.transform.rotation = Quaternion.Lerp(_ai.transform.rotation,
                Quaternion.LookRotation(-dir), Time.deltaTime * 30);

            if (attackDelayTimer >= 100)
            {
                if (!AttackingTag)
                {
                    _player.NOfGhostsAround += 1;
                }

                AttackingTag = true;

                _anim.SetTrigger("Bite Attack");

                if (_player != null)
                    _player.DealDamage(1f);

                attackDelayTimer = 0;
            }
            return Vector3.zero;
        }

        private Vector3 CirclePlayer()
        {
            Vector3 dir = _ai.transform.position - _player.transform.position;

            dir = dir.normalized * 5;

            return _player.transform.position + dir;
        }

        private Vector3 GetPlayerPosition()
        {
            return _player.gameObject.transform.position;
        }
    }
}