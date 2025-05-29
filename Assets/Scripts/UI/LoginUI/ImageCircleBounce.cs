using UnityEngine;

public class ImageCircleBounce : MonoBehaviour
{
    [SerializeField] private Vector2 movementSpeed = new Vector2(200f, 200f); // Kecepatan gerakan
    //[SerializeField] private float randomDirectionRange = 1.0f; // Range untuk arah random setelah tabrakan
    private RectTransform rectTransform;

    private Vector2 direction;
    private Rect screenBounds;
    private float radius;

    private Animator animator;
    private bool isAnimationPlaying = false;

    private void Start()
    {
        animator = GetComponent<Animator>();

        rectTransform = GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            Debug.LogError("Script ini harus ditempelkan pada UI Image dengan RectTransform.");
            return;
        }

        // Hitung radius RectTransform
        radius = rectTransform.rect.width / 2;

        // Set arah gerakan awal secara acak
        direction = Random.insideUnitCircle.normalized;

        // Hitung batas layar berdasarkan Canvas
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas == null || canvas.renderMode != RenderMode.ScreenSpaceOverlay)
        {
            Debug.LogError("Script ini memerlukan Canvas dengan RenderMode Screen Space - Overlay.");
            return;
        }

        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        screenBounds = new Rect(-canvasRect.rect.width / 2, -canvasRect.rect.height / 2, canvasRect.rect.width, canvasRect.rect.height);
    }

    private void Update()
    {
        MoveImage();
        CheckCollisionWithOthers();
    }

    private void MoveImage()
    {
        // Gerakkan UI Image berdasarkan arah dan kecepatan
        rectTransform.anchoredPosition += direction * movementSpeed * Time.deltaTime;

        // Pantulkan jika keluar dari batas layar
        if (rectTransform.anchoredPosition.x - radius < screenBounds.xMin || rectTransform.anchoredPosition.x + radius > screenBounds.xMax)
        {
            direction.x = -direction.x; // Pantulan horizontal
            rectTransform.anchoredPosition = new Vector2(
                Mathf.Clamp(rectTransform.anchoredPosition.x, screenBounds.xMin + radius, screenBounds.xMax - radius),
                rectTransform.anchoredPosition.y);
        }

        if (rectTransform.anchoredPosition.y - radius < screenBounds.yMin || rectTransform.anchoredPosition.y + radius > screenBounds.yMax)
        {
            direction.y = -direction.y; // Pantulan vertikal
            rectTransform.anchoredPosition = new Vector2(
                rectTransform.anchoredPosition.x,
                Mathf.Clamp(rectTransform.anchoredPosition.y, screenBounds.yMin + radius, screenBounds.yMax - radius));
        }
    }

    private void CheckCollisionWithOthers()
    {
        ImageCircleBounce[] allImages = FindObjectsOfType<ImageCircleBounce>();
        foreach (ImageCircleBounce other in allImages)
        {
            if (other == this) continue;

            // Hitung jarak antar pusat lingkaran
            float distance = Vector2.Distance(rectTransform.anchoredPosition, other.rectTransform.anchoredPosition);

            // Jika jarak lebih kecil dari gabungan radius, terjadi tabrakan
            if (distance < radius + other.radius)
            {
                HandleCollision(other);
            }
        }
    }

    private void HandleCollision(ImageCircleBounce other)
    {
        // Pantulkan arah gerakan ke arah random
        direction = Random.insideUnitCircle.normalized;
        other.direction = Random.insideUnitCircle.normalized;

        // Hindari gambar saling menempel setelah tabrakan
        Vector2 collisionNormal = (rectTransform.anchoredPosition - other.rectTransform.anchoredPosition).normalized;
        float overlap = (radius + other.radius) - Vector2.Distance(rectTransform.anchoredPosition, other.rectTransform.anchoredPosition);

        rectTransform.anchoredPosition += collisionNormal * overlap * 0.5f;
        other.rectTransform.anchoredPosition -= collisionNormal * overlap * 0.5f;

        // Mainkan animasi jika belum diputar
        if (animator != null && !isAnimationPlaying)
        {
            isAnimationPlaying = true; // Set flag agar animasi tidak diputar lagi
            animator.SetTrigger("PlayBounceAnimation"); // Gantilah "PlayBounceAnimation" dengan nama trigger animasi Anda

            // Reset flag setelah animasi selesai (menggunakan waktu durasi animasi)
            float animationDuration = GetAnimationClipDuration("PlayBounceAnimation"); // Fungsi untuk mendapatkan durasi animasi
            Invoke(nameof(ResetAnimationFlag), animationDuration);
        }
    }
    private void ResetAnimationFlag()
    {
        isAnimationPlaying = false; // Reset flag agar animasi bisa diputar lagi
    }
    private float GetAnimationClipDuration(string animationName)
    {
        if (animator == null) return 0f;

        // Cari klip animasi berdasarkan nama
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == animationName)
            {
                return clip.length; // Kembalikan durasi klip
            }
        }
        return 0f; // Jika klip tidak ditemukan, kembalikan 0
    }
}
