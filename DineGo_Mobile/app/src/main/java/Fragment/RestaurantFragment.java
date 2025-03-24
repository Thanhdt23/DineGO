package Fragment;

import android.os.Bundle;
import androidx.annotation.NonNull;
import androidx.fragment.app.Fragment;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.example.dinego_mobile.R;

import java.util.ArrayList;
import java.util.List;

import Adapter.RestaurantAdapter;
import Data.DatabaseHelper;
import Models.Restaurant;

public class RestaurantFragment extends Fragment {
    private RecyclerView recyclerView;
    private RestaurantAdapter adapter;
    private List<Restaurant> restaurantList = new ArrayList<>();

    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_restaurant, container, false);

        recyclerView = view.findViewById(R.id.recyclerViewRestaurants);
        recyclerView.setLayoutManager(new LinearLayoutManager(getContext()));

        adapter = new RestaurantAdapter(getContext(), restaurantList);
        recyclerView.setAdapter(adapter);

        loadRestaurants();  // Gọi phương thức để tải dữ liệu từ database

        return view;
    }

    private void loadRestaurants() {
        DatabaseHelper databaseHelper = new DatabaseHelper();
        databaseHelper.getRestaurants(restaurants -> {
            if (restaurants != null) {
                restaurantList.clear();
                restaurantList.addAll(restaurants);
                adapter.notifyDataSetChanged();
            }
        });
    }
}

