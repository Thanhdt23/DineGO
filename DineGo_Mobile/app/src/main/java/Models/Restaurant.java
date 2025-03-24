package Models;

public class Restaurant {
    private String name;
    private String address;
    private String phone;
    private String imageUrl;

    public Restaurant(String name, String address, String phone, String imageUrl) {
        this.name = name;
        this.address = address;
        this.phone = phone;
        this.imageUrl = imageUrl;
    }

    public String getName() { return name; }
    public String getAddress() { return address; }
    public String getPhone() { return phone; }
    public String getImageUrl() { return imageUrl; }
}

