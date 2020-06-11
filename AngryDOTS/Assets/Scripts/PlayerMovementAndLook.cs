using UnityEngine;

public class PlayerMovementAndLook : MonoBehaviour
{
	[Header("Camera")]
	public Camera mainCamera;

	[Header("Movement")]
	public float speed = 4.5f;
	public LayerMask whatIsGround;

	[Header("Life Settings")]
	public float playerHealth = 1f;

	[Header("Animation")]
	public Animator playerAnimator;

	public MeshRenderer mapTrans;
	public DragDir dragDir;

	public float offset;
	Rigidbody playerRigidbody;
	bool isDead;

	void Awake()
	{
		playerRigidbody = GetComponent<Rigidbody>();
	}

	void FixedUpdate()
	{
		if (isDead)
			return;

		Vector3 desiredDirection = dragDir.dir;
		MoveThePlayer(desiredDirection);
		TurnThePlayer();
		AnimateThePlayer(desiredDirection);
	}

	public void MoveThePlayer(Vector3 desiredDirection)
	{
		Vector3 movement = new Vector3(desiredDirection.x, 0f, desiredDirection.z);
		movement = movement.normalized * speed * Time.deltaTime;

		Vector3 newPos = transform.position + movement;
		float camX = Mathf.Clamp(newPos.x, mapTrans.bounds.min.x + offset,mapTrans.bounds.max.x - offset ); 
		float camZ = Mathf.Clamp(newPos.z, mapTrans.bounds.min.z + offset, mapTrans.bounds.max.z - offset);
		playerRigidbody.MovePosition(new Vector3(camX, newPos.y, camZ));
	}

	void TurnThePlayer()
	{
		Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, whatIsGround))
		{
			Vector3 playerToMouse = hit.point - transform.position;
			playerToMouse.y = 0f;
			playerToMouse.Normalize();

			Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
			playerRigidbody.MoveRotation(newRotation);
		}
	}

	void AnimateThePlayer(Vector3 desiredDirection)
	{
		if(!playerAnimator)
			return;

		Vector3 movement = new Vector3(desiredDirection.x, 0f, desiredDirection.z);
		float forw = Vector3.Dot(movement, transform.forward);
		float stra = Vector3.Dot(movement, transform.right);

		playerAnimator.SetFloat("Forward", forw);
		playerAnimator.SetFloat("Strafe", stra);
	}

	//Player Collision
	void OnTriggerEnter(Collider theCollider)
	{
		if (!theCollider.CompareTag("Enemy"))
			return;

		playerHealth--;

		if(playerHealth <= 0)
		{
			Settings.PlayerDied();
		}
	}

	public void PlayerDied()
	{
		if (isDead)
			return;

		isDead = true;

		playerAnimator.SetTrigger("Died");
		playerRigidbody.isKinematic = true;
		GetComponent<Collider>().enabled = false;
	}
}
