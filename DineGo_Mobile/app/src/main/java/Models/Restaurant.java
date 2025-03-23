package Models;

public class Restaurant {
    private String name;
    private String address;
    private String phone;
    private String type;
    private String imageUrl;

    public Restaurant(String name, String address, String type, String imageUrl) {
        this.name = name;
        this.address = address;
        this.type = type;
        this.imageUrl = imageUrl;
    }

    public String getName() { return name; }
    public String getAddress() { return address; }
    public String getPhone() { return phone; }
    public String getType() { return type; }
    public String getImageUrl() { return imageUrl; }
}

