package Adapter;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;
import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;
import com.bumptech.glide.Glide;
import com.bumptech.glide.load.engine.DiskCacheStrategy;
import com.example.dinego_mobile.R;

import org.json.JSONArray;
import org.json.JSONException;

import java.util.List;

import Models.Restaurant;

public class RestaurantAdapter extends RecyclerView.Adapter<RestaurantAdapter.ViewHolder> {
    private List<Restaurant> restaurantList;
    private Context context;

    public RestaurantAdapter(Context context, List<Restaurant> restaurantList) {
        this.context = context;
        this.restaurantList = restaurantList;
    }

    @NonNull
    @Override
    public ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_restaurant, parent, false);
        return new ViewHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull ViewHolder holder, int position) {
        Restaurant restaurant = restaurantList.get(position);

        // Lấy ảnh đầu tiên từ chuỗi JSON
        String imageListStr = restaurant.getImageUrl(); // VD: '["res1.jpeg", "res2.jpeg"]'
        String firstImage = "";

        if (imageListStr != null && !imageListStr.isEmpty()) {
            try {
                JSONArray jsonArray = new JSONArray(imageListStr);
                if (jsonArray.length() > 0) {
                    firstImage = jsonArray.getString(0); // Lấy ảnh đầu tiên
                }
            } catch (JSONException e) {
                e.printStackTrace();
            }
        }

        // Nếu firstImage vẫn rỗng, đặt ảnh mặc định
        String imageUrl;
        if (!firstImage.isEmpty()) {
            imageUrl = "E:/SE1707_Ky9/DineGo/DineGO/DineGO_Client/wwwroot/client/images/" + firstImage;
        } else {
            imageUrl = ""; // Hoặc đường dẫn ảnh mặc định
        }

        // Load ảnh bằng Glide
        Glide.with(holder.itemView.getContext())
                .load(imageUrl.isEmpty() ? R.drawable.dinego_logo : imageUrl)
                .diskCacheStrategy(DiskCacheStrategy.ALL)
                .error(R.drawable.dinego_logo) // Ảnh mặc định nếu lỗi
                .into(holder.imageView);
    }

    @Override
    public int getItemCount() {
        return restaurantList.size();
    }

    public static class ViewHolder extends RecyclerView.ViewHolder {
        TextView nameTextView, addressTextView, typeTextView;
        ImageView imageView;

        public ViewHolder(View itemView) {
            super(itemView);
            nameTextView = itemView.findViewById(R.id.restaurant_name);
            addressTextView = itemView.findViewById(R.id.restaurant_address);
            typeTextView = itemView.findViewById(R.id.restaurant_type);
            imageView = itemView.findViewById(R.id.restaurant_image);
        }
    }
}
