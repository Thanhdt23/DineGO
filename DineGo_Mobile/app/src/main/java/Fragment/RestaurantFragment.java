package Fragment;

import android.os.Bundle;
import android.text.TextUtils;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.LinearLayout;
import android.widget.SearchView;

import androidx.annotation.NonNull;
import androidx.fragment.app.Fragment;
import androidx.recyclerview.widget.GridLayoutManager;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

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
    private SearchView searchView;

    private LinearLayout allType, bbqType, hotpotType, dessertType, drinkType;


    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_restaurant, container, false);

        recyclerView = view.findViewById(R.id.recyclerViewRestaurants);

        searchView = view.findViewById(R.id.searchView);
        recyclerView.setLayoutManager(new LinearLayoutManager(getContext()));

        adapter = new RestaurantAdapter(getContext(), restaurantList);
        recyclerView.setAdapter(adapter);

        loadRestaurants();
        setupSearchView();//search by name

        //search by type
        allType = view.findViewById(R.id.allType);
        bbqType = view.findViewById(R.id.bbqType);
        hotpotType = view.findViewById(R.id.hotpotType);
        dessertType = view.findViewById(R.id.dessertType);
        drinkType = view.findViewById(R.id.drinkType);

        setupCategoryFilters();


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

    private void setupSearchView() {
        searchView.setOnQueryTextListener(new SearchView.OnQueryTextListener() {
            @Override
            public boolean onQueryTextSubmit(String query) {
                searchRestaurants(query);
                return true;
            }

            @Override
            public boolean onQueryTextChange(String newText) {
                if (TextUtils.isEmpty(newText)) {
                    loadRestaurants(); // Nếu input trống, hiển thị lại toàn bộ danh sách
                } else {
                    searchRestaurants(newText);
                }
                return true;
            }
        });
    }

    private void searchRestaurants(String query) {
        DatabaseHelper databaseHelper = new DatabaseHelper();
        databaseHelper.searchRestaurantsByName(query, filteredRestaurants -> {
            if (filteredRestaurants != null) {
                restaurantList.clear();
                restaurantList.addAll(filteredRestaurants);
                adapter.notifyDataSetChanged();
            }
        });
    }

    private void setupCategoryFilters() {
        allType.setOnClickListener(v -> loadRestaurants());
        bbqType.setOnClickListener(v -> searchRestaurantsByType("BBQ"));
        hotpotType.setOnClickListener(v -> searchRestaurantsByType("Hotpot"));
        dessertType.setOnClickListener(v -> searchRestaurantsByType("Dessert"));
        drinkType.setOnClickListener(v -> searchRestaurantsByType("Drink"));
    }

    private void searchRestaurantsByType(String type) {
        DatabaseHelper databaseHelper = new DatabaseHelper();
        databaseHelper.searchRestaurantsByType(type, filteredRestaurants -> {
            if (filteredRestaurants != null) {
                restaurantList.clear();
                restaurantList.addAll(filteredRestaurants);
                adapter.notifyDataSetChanged();
            }
        });
    }

}
