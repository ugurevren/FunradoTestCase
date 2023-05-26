using System.Collections;
using UnityEngine;

namespace Character
{
    public class EnemyController : Character
    {
        [SerializeField] private float speed = 5;   // The speed of the character.
        [SerializeField] private  float waitTime = .3f;  // The time the character waits at each waypoint.
        [SerializeField] private float turnSpeed = 90;  // The turn speed of the character.
        private bool _isMoving; // The state of the character.
        
        private Animator _animator; // The animator component of the character.
        
        [SerializeField] private Transform pathHolder;  // The path that the character follows.
        
        [SerializeField] private Light spotlight;   // The spotlight of the character. Visualization of the view distance.
        [SerializeField] private float viewDistance;    // The view distance of the character.
        [SerializeField] private LayerMask viewMask;    // The view mask of the character.
        private float _viewAngle;   // The view angle of the character.
        private Color _originalSpotlightColor;  // The original color of the spotlight.
        
        private Transform _playerTransform; // The transform of the player.
        
        private void Awake()
        {
            floatingTextObject = GetComponentInChildren<FloatingText>().gameObject; // Get the floating text object.
        }
        private void Start()
        {
            _animator = GetComponent<Animator>();   // Get the animator component.
            _playerTransform = GameObject.FindGameObjectWithTag ("Player").transform;   // Get the transform of the player.
            floatingText= floatingTextObject.GetComponent<FloatingText>();
            UpdateLevelText(level); // Update the level text.
            _viewAngle  = spotlight.spotAngle;  // Get the view angle of the character.
            _originalSpotlightColor = spotlight.color;  // Get the original color of the spotlight.
            Vector3[] waypoints = new Vector3[pathHolder.childCount];  // Create a vector array for the waypoints.
            // Get the waypoints from the path holder.
            for (int i = 0; i < waypoints.Length; i++) {
                waypoints [i] = pathHolder.GetChild (i).position;
                waypoints [i] = new Vector3 (waypoints [i].x, transform.position.y, waypoints [i].z);
            }
            StartCoroutine (FollowPath (waypoints));    // Start following the path.
        }
        private void Update()
        {
            _animator.SetBool("isMoving", _isMoving);   // Set the isMoving parameter of the animator.
            if (CanSeePlayer ()) {
                // If the character can see the player, set the color of the spotlight to red.
                spotlight.color = Color.red;
            } else {
                // If the character cannot see the player, set the color of the spotlight to the original color.
                spotlight.color = _originalSpotlightColor;
            }
        }
        bool CanSeePlayer() {
            if (Vector3.Distance(transform.position,_playerTransform.position) < viewDistance) {
                // If the distance between the character and the player is less than the view distance, check the angle between the character and the player.
                Vector3 dirToPlayer = (_playerTransform.position - transform.position).normalized;
                float angleBetweenGuardAndPlayer = Vector3.Angle (transform.forward, dirToPlayer);
                if (angleBetweenGuardAndPlayer < _viewAngle / 2f) {
                    // If the angle between the character and the player is less than the half of the view angle, check if the character can see the player.
                    if (!Physics.Linecast (transform.position, _playerTransform.position, viewMask)) {
                        // If the character can see the player, return true.
                        return true;
                    }
                }
            }
            // If the character cannot see the player, return false.
            return false;
        }
        private IEnumerator FollowPath(Vector3[] waypoints) {
            // Method to follow path
            transform.position = waypoints [0]; // Set the position of the character to the first waypoint.
            int targetWaypointIndex = 1;    // Set the target waypoint index to 1.
            Vector3 targetWaypoint = waypoints [targetWaypointIndex];   // Set the target waypoint to the second waypoint.
            transform.LookAt (targetWaypoint);  // Look at the target waypoint.
            while (true) {
                // While the character is not at the target waypoint, move towards the target waypoint.
                transform.position = Vector3.MoveTowards (transform.position, targetWaypoint, speed * Time.deltaTime);
                _isMoving = true;
                
                if (transform.position == targetWaypoint) {
                    // If the character is at the target waypoint, set the state of the character to false,
                    // set the target waypoint index to the next waypoint, set the target waypoint to the next waypoint,
                    // wait for the wait time, and turn to face the target waypoint.
                    _isMoving = false;
                    targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
                    targetWaypoint = waypoints [targetWaypointIndex];
                    yield return new WaitForSeconds (waitTime);
                    yield return StartCoroutine (TurnToFace (targetWaypoint));
                }
                yield return null;
            }
        }

        private IEnumerator TurnToFace(Vector3 lookTarget) {
            // Method to turn to face the target waypoint.
            Vector3 dirToLookTarget = (lookTarget - transform.position).normalized; // Get the direction to the target waypoint.
            float targetAngle = 90 - Mathf.Atan2 (dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;    // Get the target angle.
            while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f) {
                // While the angle between the character and the target waypoint is greater than 0.05f, turn towards the target waypoint.
                float angle = Mathf.MoveTowardsAngle (transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
                transform.eulerAngles = Vector3.up * angle;
                yield return null;
            }
        }
        public override void Combat(Collider other)
        {
            int playerLevel = other.GetComponent<Character>().level;    // Get the level of the player.
            if (playerLevel > level)
            {
                // If the level of the player is greater than the level of the character, set the level of the character to the level of the player.
                gameObject.GetComponent<Animator>().enabled = false;
                // beklet ve sil
                Destroy(gameObject, 1f);
            }
        }
        private void OnDrawGizmos() {
            // Method to draw the path and the view distance of the character.
            Vector3 startPosition = pathHolder.GetChild (0).position;
            Vector3 previousPosition = startPosition;
            foreach (Transform waypoint in pathHolder) {
                Gizmos.DrawSphere (waypoint.position, .3f);
                Gizmos.DrawLine (previousPosition, waypoint.position);
                previousPosition = waypoint.position;
            }
            Gizmos.DrawLine (previousPosition, startPosition);
            Gizmos.color = Color.red;
            Gizmos.DrawRay (transform.position, transform.forward * viewDistance);
        }
    }
}
