package Adapter;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.example.dinego_mobile.R;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.List;
import java.util.Locale;

import Models.Reservation;

public class ReservationAdapter extends RecyclerView.Adapter<ReservationAdapter.ViewHolder> {
    private List<Reservation> reservationList;
    private Context context;
    private OnItemClickListener listener;

    public interface OnItemClickListener {
        void onViewDetailsClick(Reservation reservation);
    }

    public ReservationAdapter(Context context, List<Reservation> reservationList, OnItemClickListener listener) {
        this.context = context;
        this.reservationList = reservationList;
        this.listener = listener;
    }

    @NonNull
    @Override
    public ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(parent.getContext())
                .inflate(R.layout.item_reservation, parent, false);
        return new ViewHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull ViewHolder holder, int position) {
        Reservation reservation = reservationList.get(position);

        // Format lại ngày
        SimpleDateFormat inputFormat = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss.SSSSSSS", Locale.getDefault());
        SimpleDateFormat outputFormat = new SimpleDateFormat("yyyy-MM-dd HH:mm", Locale.getDefault());

        try {
            Date date = inputFormat.parse(reservation.getReDate());
            holder.reservationDate.setText("Date: " + outputFormat.format(date));
        } catch (ParseException e) {
            holder.reservationDate.setText("Date: " + reservation.getReDate()); // Giữ nguyên nếu lỗi
        }

        holder.reservationStatus.setText("Status: " + reservation.getReStatus());
        holder.reservationQuantity.setText("Quantity: " + reservation.getReQuantity());

        if (reservation.getReNote() != null && !reservation.getReNote().isEmpty()) {
            holder.reservationNote.setText("Note: " + reservation.getReNote());
            holder.reservationNote.setVisibility(View.VISIBLE);
        } else {
            holder.reservationNote.setVisibility(View.GONE);
        }

        holder.btnViewDetails.setOnClickListener(v -> {
            if (listener != null) {
                listener.onViewDetailsClick(reservation);
            }
        });
    }


    @Override
    public int getItemCount() {
        return reservationList.size();
    }

    public static class ViewHolder extends RecyclerView.ViewHolder {
        TextView reservationDate, reservationStatus, reservationQuantity, reservationNote;
        Button btnViewDetails;

        public ViewHolder(View itemView) {
            super(itemView);
            reservationDate = itemView.findViewById(R.id.reservation_date);
            reservationStatus = itemView.findViewById(R.id.reservation_status);
            reservationQuantity = itemView.findViewById(R.id.reservation_quantity);
            reservationNote = itemView.findViewById(R.id.reservation_note);
            btnViewDetails = itemView.findViewById(R.id.btn_view_details);
        }
    }
}