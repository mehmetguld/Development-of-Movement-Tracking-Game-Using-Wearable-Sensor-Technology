using UnityEngine;
public class ButtonActivator : MonoBehaviour
{
    public mpu6050 mpu6050;  // MPU6050 sens�r�n� al
    private float button1State; // 1. butonun durumu
    private bool isInsideCollider = false; // Objeler collider s�n�rlar� i�inde mi?
    private bool hasStarted = false;
    private int previousButtonState = 1;
    private Collider2D otherCollider; // �arp��t��� collider objesi

    void Start()
    {
        if (mpu6050 == null)
        {
            mpu6050 = FindObjectOfType<mpu6050>(); // MPU6050 objesini bul
        }
    }

    void Update()
    {
        // �lk veri al�nm�� m� kontrol et
        if (!hasStarted && mpu6050.buttonData.x != 0)
        {
            hasStarted = true;
            return;
        }

        if (hasStarted && isInsideCollider)
        {
            button1State = mpu6050.buttonData.x; // buttonData.x buton durumu (0 veya 1)
            int currentButtonState = (int)button1State;

            // Sadece buton durumu 1'den 0'a de�i�ti�inde tetikle
            if (previousButtonState == 1 && currentButtonState == 0)
            {
                if (otherCollider != null)
                {
                    MainMenuController armutScript = otherCollider.GetComponent<MainMenuController>();
                    if (armutScript != null)
                    {
                        Debug.Log("Button durumu 0, Scene de�i�tirilecek.");
                        armutScript.SceneChange(); // SceneChange metodunu �a��r
                    }
                }
            }

            // �nceki buton durumunu g�ncelle
            previousButtonState = currentButtonState;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Hangi objeyle �arp��t���n� g�rmek i�in log
        Debug.Log("�arp��ma oldu: " + other.name);
        // E�er �arp��an objenin tag'� "Gun" ise, objemiz bu collider s�n�rlar� i�inde
        if (other.CompareTag("Gun"))
        {
            isInsideCollider = true;  // Obje collider s�n�rlar� i�ine girdi
            otherCollider = other; // �arp��t��� collider'� kaydet
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // E�er obje collider'dan ��karsa
        if (other.CompareTag("Gun"))
        {
            isInsideCollider = false;  // Obje collider s�n�rlar� d���na ��kt�
            otherCollider = null; // �arp��t��� collider'� null yap
        }
    }
}

//using UnityEngine;

//public class ButtonActivator : MonoBehaviour
//{
//    public mpu6050 mpu6050;  // MPU6050 sens�r�n� al
//    private float button1State; // 1. butonun durumu
//    private bool isInsideCollider = false; // Objeler collider s�n�rlar� i�inde mi?

//    private Collider2D otherCollider; // �arp��t��� collider objesi

//    void Start()
//    {
//        if (mpu6050 == null)
//        {
//            mpu6050 = FindObjectOfType<mpu6050>(); // MPU6050 objesini bul
//        }
//    }

//    void Update()
//    {
//        // Buton durumu her frame'de g�ncellenir
//        button1State = mpu6050.buttonData.x; // buttonData.x buton durumu (0 veya 1)

//        // E�er obje collider s�n�rlar� i�indeyse ve buton durumu "0" ise
//        if (isInsideCollider && button1State == 0)
//        {
//            // �arp��t��� objenin script'ini bul
//            if (otherCollider != null)
//            {
//                MainMenuController armutScript = otherCollider.GetComponent<MainMenuController>();

//                if (armutScript != null)
//                {
//                    Debug.Log("Button durumu 0, Scene de�i�tirilecek.");
//                    armutScript.SceneChange(); // SceneChange metodunu �a��r
//                }
//            }
//        }
//    }

//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        // Hangi objeyle �arp��t���n� g�rmek i�in log
//        Debug.Log("�arp��ma oldu: " + other.name);

//        // E�er �arp��an objenin tag'� "Gun" ise, objemiz bu collider s�n�rlar� i�inde
//        if (other.CompareTag("Gun"))
//        {
//            isInsideCollider = true;  // Obje collider s�n�rlar� i�ine girdi
//            otherCollider = other; // �arp��t��� collider'� kaydet
//        }
//    }

//    private void OnTriggerExit2D(Collider2D other)
//    {
//        // E�er obje collider'dan ��karsa
//        if (other.CompareTag("Gun"))
//        {
//            isInsideCollider = false;  // Obje collider s�n�rlar� d���na ��kt�
//            otherCollider = null; // �arp��t��� collider'� null yap
//        }
//    }
//}




//using Unity.Burst.Intrinsics;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;

//public class ButtonActivator : MonoBehaviour
//{
//    private void OnTriggerEnter2D(Collider2D other)
//    {

//        Debug.Log("�arp��ma oldu: " + other.name); // Hangi objeyle �arp��t���n� g�rmek i�in
//        if (other.CompareTag("Gun"))
//        {
//            // Armut objesi ile �arp��ma oldu
//            MainMenuController armutScript = other.GetComponent<MainMenuController>();
//            if (armutScript != null)
//            {
//                Debug.Log("Armut scripti bulundu. Scene de�i�tirilecek.");
//                armutScript.SceneChange(); // Armut objesindeki metodu �a��r
//            }
//        }
//    }
//}
