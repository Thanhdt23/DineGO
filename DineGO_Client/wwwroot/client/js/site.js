// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $('.owl-carousel').owlCarousel({
        loop: true,
        margin: 10,
        nav: true,
        autoplay: true,
        autoplayTimeout: 3000,
        autoplayHoverPause: true,
        responsive: {
            0: {
                items: 1
            },
            600: {
                items: 2
            },
            1000: {
                items: 3
            }
        }
    })
})


//Product - Start - Thang

    const radios = document.querySelectorAll('input[type="radio"]');
    const regionButton = document.querySelector('.region-display');

    // Lắng nghe sự kiện thay đổi radio
    radios.forEach((radio) => {
        radio.addEventListener('change', () => {
            // Cập nhật nội dung nút dựa trên nhãn của radio được chọn
            regionButton.textContent = document.querySelector(`label[for="${radio.id}"]`).textContent;
        });
    });

    const rangeInput = document.querySelectorAll(".range-input input"),
        priceInput = document.querySelectorAll(".price-input input"),
        range = document.querySelector(".slider .progress");
    let priceGap = 1000;

    priceInput.forEach(input => {
        input.addEventListener("input", e => {
            let minPrice = parseInt(priceInput[0].value),
                maxPrice = parseInt(priceInput[1].value);

            if ((maxPrice - minPrice >= priceGap) && maxPrice <= rangeInput[1].max) {
                if (e.target.className === "input-min") {
                    rangeInput[0].value = minPrice;
                    range.style.left = ((minPrice / rangeInput[0].max) * 100) + "%";
                } else {
                    rangeInput[1].value = maxPrice;
                    range.style.right = 100 - (maxPrice / rangeInput[1].max) * 100 + "%";
                }
            }
        });
    });

    rangeInput.forEach(input => {
        input.addEventListener("input", e => {
            let minVal = parseInt(rangeInput[0].value),
                maxVal = parseInt(rangeInput[1].value);

            if ((maxVal - minVal) < priceGap) {
                if (e.target.className === "range-min") {
                    rangeInput[0].value = maxVal - priceGap
                } else {
                    rangeInput[1].value = minVal + priceGap;
                }
            } else {
                priceInput[0].value = minVal;
                priceInput[1].value = maxVal;
                range.style.left = ((minVal / rangeInput[0].max) * 100) + "%";
                range.style.right = 100 - (maxVal / rangeInput[1].max) * 100 + "%";
            }
        });
    });

//Product - End - Thang




//Profile_Header _ Thang _ Start
  let timeout;
  document.getElementById("profileContainer").addEventListener("mouseenter", function () {
    clearTimeout(timeout);
    document.getElementById("profileMenu").style.display = "block";
  });

  document.getElementById("profileContainer").addEventListener("mouseleave", function () {
    timeout = setTimeout(function () {
      document.getElementById("profileMenu").style.display = "none";
    }, 200);
  });

  document.getElementById("profileMenu").addEventListener("mouseenter", function () {
    clearTimeout(timeout);
  });

  document.getElementById("profileMenu").addEventListener("mouseleave", function () {
    timeout = setTimeout(function () {
      document.getElementById("profileMenu").style.display = "none";
    }, 200);
  });
  //Add js _ Phuonghh _ Start
  function toggleDropdown() {
        let dropdown = document.getElementById("profileDropdown");
        let arrow = document.querySelector(".dropdown-arrow");
        dropdown.style.display = dropdown.style.display === "block" ? "none" : "block";
        arrow.style.transform = dropdown.style.display === "block" ? "rotate(180deg)" : "rotate(0deg)";
    }

    // Ẩn dropdown khi click ra ngoài
    document.addEventListener("click", function (event) {
        let profileInfo = document.querySelector(".profile-info");
        let dropdown = document.getElementById("profileDropdown");

        if (!profileInfo.contains(event.target)) {
            dropdown.style.display = "none";
            document.querySelector(".dropdown-arrow").style.transform = "rotate(0deg)";
        }
    });

  //Add js _ Phuonghh _ End
//Profile_Header _ Thang _ End


//Image_Profile _ Thang _ Start
function previewImage(event) {
    const profilePic = document.querySelector(".profile-pic");
    const file = event.target.files[0];
    if (file) {
        const reader = new FileReader();
        reader.onload = function (e) {
            profilePic.style.backgroundImage = `url(${e.target.result})`;
            profilePic.textContent = "";
        };
        reader.readAsDataURL(file);
    }
}
//Image_Profile _ Thang _ End


