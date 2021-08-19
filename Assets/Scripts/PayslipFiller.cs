using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PayslipFiller : MonoBehaviour
{
    public Text overtimePay, totalPay, comment;
    public Animator anim;

    public float basePay;
    public Vector2 overtimePayRange;
    public string[] comments;

    public void SetActive (bool active) 
    {
        gameObject.SetActive (active);
        anim.SetTrigger ("Animate");
    }

    public void Fill (float winRatio)
    {
        float overtime = Mathf.Lerp (overtimePayRange.x, overtimePayRange.y, winRatio);
        float total = basePay + overtime;

        overtimePay.text = overtime.ToString ("$0.00");
        totalPay.text = total.ToString ("$0.00");

        int commentIndex = Mathf.CeilToInt (winRatio / 0.33f);
        comment.text = comments[commentIndex];
    }
}
