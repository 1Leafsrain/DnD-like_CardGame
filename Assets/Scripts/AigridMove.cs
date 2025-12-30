using System.Collections.Generic;
using System.Collections;

using UnityEngine;


public class AigridMove : MonoBehaviour
{
    
        public float moveSpeed = 5f;
        public Transform movePoint;
        public int action = 3; // Jumlah langkah yang bisa dilakukan per giliran
        public LayerMask stop; // Layer yang menghalangi pergerakan
        public Transform playerTarget; // Target yang akan dikejar (player)
        public float decisionInterval = 1f; // Interval waktu untuk mengambil keputusan baru

    public bool canMove;

    public GameObject player;
    public GameObject enemy;
    public int jarakArea;
    public bool masukJarak;

    public EnemyCard enemyCard;

    private float decisionTimer = 0f;
        private Vector2Int lastPlayerGridPos; // Posisi grid terakhir player

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        enemyCard = GetComponentInParent<EnemyCard>();
        masukJarak = false;
            player = GameObject.FindGameObjectWithTag("Player");
            enemy = this.gameObject;
            movePoint.parent = null;

            // Jika playerTarget tidak di-set manual, cari GameObject dengan tag "Player"
            if (playerTarget == null)
            {
                GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
                if (playerObj != null)
                {
                    playerTarget = playerObj.transform;
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            // Gerakkan AI menuju movePoint
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position,
                moveSpeed * Time.deltaTime);

            // Jika AI sudah dekat dengan movePoint (hampir sampai di grid tujuan)
            if (Vector3.Distance(transform.position, movePoint.position) <= 0.5f)
            {
                // Update timer untuk pengambilan keputusan
                decisionTimer += Time.deltaTime;

                // Jika timer sudah mencapai interval dan masih ada action tersisa
                if (decisionTimer >= decisionInterval && action > 0)
                {
                    decisionTimer = 0f; // Reset timer
                    if(canMove == true)
                    {
                        CalculateAndMove(); // Hitung dan lakukan pergerakan
                        CalculateAttack();
                    }
                    
                }
            }
        }

        void CalculateAndMove()
        {
            if (playerTarget == null) return;

            // Konversi posisi ke grid (menggunakan rounding untuk mendapatkan posisi grid)
            Vector2Int aiGridPos = new Vector2Int(
                Mathf.RoundToInt(movePoint.position.x),
                Mathf.RoundToInt(movePoint.position.y)
            );

            Vector2Int playerGridPos = new Vector2Int(
                Mathf.RoundToInt(playerTarget.position.x),
                Mathf.RoundToInt(playerTarget.position.y)
            );

            // Hitung jarak antara AI dan player
            Vector2Int distance = playerGridPos - aiGridPos;

            // Prioritaskan gerakan berdasarkan jarak terbesar
            // Ini membuat AI bergerak lebih efisien menuju player
            if (Mathf.Abs(distance.x) > Mathf.Abs(distance.y))
            {
                // Jika jarak horizontal lebih besar, gerak horizontal dulu
                if (distance.x > 0)
                {
                    // Coba gerak ke kanan
                    if (!Physics2D.OverlapCircle(movePoint.position + Vector3.right, 0.2f, stop))
                    {
                        action -= 1;
                        movePoint.position += Vector3.right;
                        return; // Keluar setelah berhasil bergerak
                    }
                }
                else if (distance.x < 0)
                {
                    // Coba gerak ke kiri
                    if (!Physics2D.OverlapCircle(movePoint.position + Vector3.left, 0.2f, stop))
                    {
                        action -= 1;
                        movePoint.position += Vector3.left;
                        return;
                    }
                }

                // Jika gerakan horizontal terhalang, coba gerak vertikal
                if (distance.y > 0)
                {
                    if (!Physics2D.OverlapCircle(movePoint.position + Vector3.up, 0.2f, stop))
                    {
                        action -= 1;
                        movePoint.position += Vector3.up;
                        return;
                    }
                }
                else if (distance.y < 0)
                {
                    if (!Physics2D.OverlapCircle(movePoint.position + Vector3.down, 0.2f, stop))
                    {
                        action -= 1;
                        movePoint.position += Vector3.down;
                        return;
                    }
                }
            }
            else
            {
                // Jika jarak vertikal lebih besar, gerak vertikal dulu
                if (distance.y > 0)
                {
                    // Coba gerak ke atas
                    if (!Physics2D.OverlapCircle(movePoint.position + Vector3.up, 0.2f, stop))
                    {
                        action -= 1;
                        movePoint.position += Vector3.up;
                        return;
                    }
                }
                else if (distance.y < 0)
                {
                    // Coba gerak ke bawah
                    if (!Physics2D.OverlapCircle(movePoint.position + Vector3.down, 0.2f, stop))
                    {
                        action -= 1;
                        movePoint.position += Vector3.down;
                        return;
                    }
                }

                // Jika gerakan vertikal terhalang, coba gerak horizontal
                if (distance.x > 0)
                {
                    if (!Physics2D.OverlapCircle(movePoint.position + Vector3.right, 0.2f, stop))
                    {
                        action -= 1;
                        movePoint.position += Vector3.right;
                        return;
                    }
                }
                else if (distance.x < 0)
                {
                    if (!Physics2D.OverlapCircle(movePoint.position + Vector3.left, 0.2f, stop))
                    {
                        action -= 1;
                        movePoint.position += Vector3.left;
                        return;
                    }
                }
            }

            // Jika semua arah terhalang, AI akan tetap di posisi saat ini
            // (tidak mengurangi action karena tidak bergerak)
        }

        // Method untuk reset action (misal di setiap giliran baru)
        public void ResetAction()
        {
            action = 3; // Atau nilai default yang diinginkan
        }

        // Method untuk mengatur target secara manual
        public void SetTarget(Transform newTarget)
        {
            playerTarget = newTarget;
        }

        // Visualisasi di Editor (opsional, membantu debugging)
        void OnDrawGizmos()
        {
            if (movePoint != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(movePoint.position, 0.2f);

                if (playerTarget != null)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(transform.position, playerTarget.position);
                }
            }
        }

    void CalculateAttack()
    {
        if (enemy == null) return;

        // Konversi posisi ke grid (menggunakan rounding untuk mendapatkan posisi grid)
        Vector2Int aiGridPos = new Vector2Int(
            Mathf.RoundToInt(player.transform.position.x),
            Mathf.RoundToInt(player.transform.position.y)
        );

        Vector2Int playerGridPos = new Vector2Int(
            Mathf.RoundToInt(enemy.transform.position.x),
            Mathf.RoundToInt(enemy.transform.position.y)
        );

        // Hitung jarak antara AI dan player
        Vector2Int distance = playerGridPos - aiGridPos;

        // Prioritaskan gerakan berdasarkan jarak terbesar
        // Ini membuat AI bergerak lebih efisien menuju player
        if (distance.sqrMagnitude < (jarakArea * jarakArea))
        {
            player.GetComponent<Card>().TakeDamage(enemyCard.damage);
        }
        else
        {
            masukJarak = false;
        }
    }
}

