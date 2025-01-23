using UnityEngine;
public class ButtonActivator : MonoBehaviour
{
    public mpu6050 mpu6050;  // MPU6050 sensörünü al
    private float button1State; // 1. butonun durumu
    private bool isInsideCollider = false; // Objeler collider sýnýrlarý içinde mi?
    private bool hasStarted = false;
    private int previousButtonState = 1;
    private Collider2D otherCollider; // Çarpýþtýðý collider objesi

    void Start()
    {
        if (mpu6050 == null)
        {
            mpu6050 = FindObjectOfType<mpu6050>(); // MPU6050 objesini bul
        }
    }

    void Update()
    {
        // Ýlk veri alýnmýþ mý kontrol et
        if (!hasStarted && mpu6050.buttonData.x != 0)
        {
            hasStarted = true;
            return;
        }

        if (hasStarted && isInsideCollider)
        {
            button1State = mpu6050.buttonData.x; // buttonData.x buton durumu (0 veya 1)
            int currentButtonState = (int)button1State;

            // Sadece buton durumu 1'den 0'a deðiþtiðinde tetikle
            if (previousButtonState == 1 && currentButtonState == 0)
            {
                if (otherCollider != null)
                {
                    MainMenuController armutScript = otherCollider.GetComponent<MainMenuController>();
                    if (armutScript != null)
                    {
                        Debug.Log("Button durumu 0, Scene deðiþtirilecek.");
                        armutScript.SceneChange(); // SceneChange metodunu çaðýr
                    }
                }
            }

            // Önceki buton durumunu güncelle
            previousButtonState = currentButtonState;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Hangi objeyle çarpýþtýðýný görmek için log
        Debug.Log("Çarpýþma oldu: " + other.name);
        // Eðer çarpýþan objenin tag'ý "Gun" ise, objemiz bu collider sýnýrlarý içinde
        if (other.CompareTag("Gun"))
        {
            isInsideCollider = true;  // Obje collider sýnýrlarý içine girdi
            otherCollider = other; // Çarpýþtýðý collider'ý kaydet
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Eðer obje collider'dan çýkarsa
        if (other.CompareTag("Gun"))
        {
            isInsideCollider = false;  // Obje collider sýnýrlarý dýþýna çýktý
            otherCollider = null; // Çarpýþtýðý collider'ý null yap
        }
    }
}

//using UnityEngine;

//public class ButtonActivator : MonoBehaviour
//{
//    public mpu6050 mpu6050;  // MPU6050 sensörünü al
//    private float button1State; // 1. butonun durumu
//    private bool isInsideCollider = false; // Objeler collider sýnýrlarý içinde mi?

//    private Collider2D otherCollider; // Çarpýþtýðý collider objesi

//    void Start()
//    {
//        if (mpu6050 == null)
//        {
//            mpu6050 = FindObjectOfType<mpu6050>(); // MPU6050 objesini bul
//        }
//    }

//    void Update()
//    {
//        // Buton durumu her frame'de güncellenir
//        button1State = mpu6050.buttonData.x; // buttonData.x buton durumu (0 veya 1)

//        // Eðer obje collider sýnýrlarý içindeyse ve buton durumu "0" ise
//        if (isInsideCollider && button1State == 0)
//        {
//            // Çarpýþtýðý objenin script'ini bul
//            if (otherCollider != null)
//            {
//                MainMenuController armutScript = otherCollider.GetComponent<MainMenuController>();

//                if (armutScript != null)
//                {
//                    Debug.Log("Button durumu 0, Scene deðiþtirilecek.");
//                    armutScript.SceneChange(); // SceneChange metodunu çaðýr
//                }
//            }
//        }
//    }

//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        // Hangi objeyle çarpýþtýðýný görmek için log
//        Debug.Log("Çarpýþma oldu: " + other.name);

//        // Eðer çarpýþan objenin tag'ý "Gun" ise, objemiz bu collider sýnýrlarý içinde
//        if (other.CompareTag("Gun"))
//        {
//            isInsideCollider = true;  // Obje collider sýnýrlarý içine girdi
//            otherCollider = other; // Çarpýþtýðý collider'ý kaydet
//        }
//    }

//    private void OnTriggerExit2D(Collider2D other)
//    {
//        // Eðer obje collider'dan çýkarsa
//        if (other.CompareTag("Gun"))
//        {
//            isInsideCollider = false;  // Obje collider sýnýrlarý dýþýna çýktý
//            otherCollider = null; // Çarpýþtýðý collider'ý null yap
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

//        Debug.Log("Çarpýþma oldu: " + other.name); // Hangi objeyle çarpýþtýðýný görmek için
//        if (other.CompareTag("Gun"))
//        {
//            // Armut objesi ile çarpýþma oldu
//            MainMenuController armutScript = other.GetComponent<MainMenuController>();
//            if (armutScript != null)
//            {
//                Debug.Log("Armut scripti bulundu. Scene deðiþtirilecek.");
//                armutScript.SceneChange(); // Armut objesindeki metodu çaðýr
//            }
//        }
//    }
//}
