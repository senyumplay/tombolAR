using UnityEngine;

/// <summary>
/// Disimpan di prefab gambar. Menyimpan daftar backsound
/// dan menyediakan helper untuk mengambil satu clip acak.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class BackgroundImage : MonoBehaviour
{
    [Header("Back‑sound List (optional)")]
    [SerializeField] private AudioClip[] backsoundList;

    /// <summary>Dipanggil oleh loader untuk menetapkan backsound khusus file ini.</summary>
    public void SetBacksounds(AudioClip[] clips) => backsoundList = clips;

    /// <summary>Ambil satu backsound acak; null jika list kosong.</summary>
    public AudioClip GetRandomClip()
    {
        return backsoundList != null && backsoundList.Length > 0
            ? backsoundList[Random.Range(0, backsoundList.Length)]
            : null;
    }
}
