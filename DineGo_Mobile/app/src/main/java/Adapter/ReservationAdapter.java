package Adapter;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;


import java.util.List;

import Models.Reservation;

public class ReservationAdapter extends RecyclerView.Adapter<ReservationAdapter.ViewHolder> {
    private List<Models.Reservation> reservationList;
    private Context context;
    private OnItemClickListener listener;

    public interface OnItemClickListener {
        void onViewDetailsClick(Models.Reservation reservation);
    }

    public ReservationAdapter(Context context, List<Reservation> reservationList) {
        this.context = context;
        this.reservationList = reservationList;
        this.listener = listener;
    }

    @NonNull
    @Override
    public ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(parent.getContext()).inflate(com.example.dinego_mobile.R.layout.item_reservation, parent, false);
        return new ViewHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull ViewHolder holder, int position) {
        Reservation reservation = reservationList.get(position);


        holder.reservation_date.setText("Date: " + reservation.getReDate());
        holder.reservation_quantity.setText("Quantity: " + reservation.getReQuantity());
        holder.reservation_status.setText("Status " + reservation.getReStatus());



        // Xử lý khi click vào nút "View Details"
        holder.btn_view_details.setOnClickListener(v -> listener.onViewDetailsClick(reservation));
    }

    @Override
    public int getItemCount() {
        return reservationList.size();
    }

    public static class ViewHolder extends RecyclerView.ViewHolder {
        TextView reservation_date, reservation_quantity, reservation_status;
        ImageView reservation_image;
        Button btn_view_details;

        public ViewHolder(View itemView) {
            super(itemView);

            reservation_date = itemView.findViewById(com.example.dinego_mobile.R.id.reservation_date);
            reservation_quantity = itemView.findViewById(com.example.dinego_mobile.R.id.reservation_quantity);
            reservation_status = itemView.findViewById(com.example.dinego_mobile.R.id.reservation_status);
            reservation_image = itemView.findViewById(com.example.dinego_mobile.R.id.reservation_image);
            btn_view_details = itemView.findViewById(com.example.dinego_mobile.R.id.btn_view_details);
        }
    }
}
