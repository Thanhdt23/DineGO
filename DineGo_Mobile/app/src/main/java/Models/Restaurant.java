package Models;

public class Restaurant {
    private int id;
    private String name;
    private String address;
    private String phone;
    private String type;
    private String imageUrl;
    private String information;
    private Double price;

    public Restaurant(int id,String name, String address, String type, String imageUrl) {
        this.id=id;
        this.name = name;
        this.address = address;
        this.type = type;
        this.imageUrl = imageUrl;
    }

    public Restaurant(int id, String name, String address, String type, String imageUrl, String information, Double price) {
        this.id = id;
        this.name = name;
        this.address = address;
        this.type = type;
        this.imageUrl = imageUrl;
        this.information = information;
        this.price = price;
    }

    public int getId() { return id; }
    public String getName() { return name; }
    public String getAddress() { return address; }
    public String getPhone() { return phone; }
    public String getType() { return type; }
    public String getImageUrl() { return imageUrl; }
    @Override
    public String toString() {
        return name; // Hiển thị tên nhà hàng trong Spinner
    }

    public String getInformation() { return information; }
    public Double getPrice() { return price; }
}

