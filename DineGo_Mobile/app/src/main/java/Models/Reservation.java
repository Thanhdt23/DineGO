package Models;

import java.util.Date;

public class Reservation {
    private int reId;
    private String reStatus;
    private String reDate;
    private String reQuantity;
    private String reNote;
    private int cusId;
    public int resId;

    public Reservation(int reId, String reStatus, String reDate, String reQuantity, String reNote, int cusId ,int resId) {
        this.reId = reId;
        this.reStatus = "Pending";
        this.reDate = reDate;
        this.reQuantity = reQuantity;
        this.reNote = reNote;
        this.cusId = cusId;
        this.resId = resId;
    }
    public int getReId() { return reId; }
    public String getReStatus() { return reStatus; }

    public String getReDate() {return reDate;}
    public String getReQuantity() { return reQuantity; }
    public String getReNote() { return reNote; }
    public int getCusId() { return cusId; }

    public int getResId() { return resId; }


}
