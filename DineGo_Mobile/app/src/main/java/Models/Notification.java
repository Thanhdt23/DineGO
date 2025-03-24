package Models;

public class Notification {
    private int notiId;
    private String notiTitle;
    private String notiContent;
    private String notiType;
    private String notiDate;
    private String notiStatus;
    private int customerId;
    private boolean isRead;

    public Notification(int notiId, String notiTitle, String notiContent, String notiType, String notiDate, String notiStatus, int customerId, boolean isRead) {
        this.notiId = notiId;
        this.notiTitle = notiTitle;
        this.notiContent = notiContent;
        this.notiType = notiType;
        this.notiDate = notiDate;
        this.notiStatus = notiStatus;
        this.customerId = customerId;
        this.isRead = isRead;
    }

    public boolean getIsRead() {
        return isRead;
    }

    public int getNotiId() {
        return notiId;
    }

    public String getNotiTitle() {
        return notiTitle;
    }

    public String getNotiContent() {
        return notiContent;
    }

    public String getNotiType() {
        return notiType;
    }

    public String getNotiDate() {
        return notiDate;
    }

    public String getNotiStatus() {
        return notiStatus;
    }

    public int getCustomerId() {
        return customerId;
    }
}
