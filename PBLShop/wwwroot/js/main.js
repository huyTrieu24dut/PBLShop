//header thay đổi quảng cáo mỗi 3s
var adsElement = document.getElementById("ads");
var adTexts = adsElement.querySelectorAll(".ad-text");
var currentAdIndex = 0;

function displayNextAd() {
    adTexts.forEach(function(adText) {
        adText.style.display = "none"; // Ẩn tất cả các quảng cáo trước khi hiển thị quảng cáo tiếp theo
    });
    adTexts[currentAdIndex].style.display = "inline"; // Hiển thị quảng cáo tiếp theo
    currentAdIndex = (currentAdIndex + 1) % adTexts.length;
}

displayNextAd();
setInterval(displayNextAd, 3000);
//header thay đổi quảng cáo mỗi 3s

//hiển thị searchbar
var searchButton = document.getElementById('searchButton');
var searchBar = document.getElementById('searchBar');
var exitSearchBar = document.getElementById('exit-searchBar');
searchButton.addEventListener('click', function(event) {
    event.stopPropagation(); //không lan truyền sự kiện đi nơi khác
    turnSearchBar(); // Gọi hàm để hiển thị hoặc ẩn thanh tìm kiếm
});
exitSearchBar.addEventListener('click', function(event) {
    event.stopPropagation(); //không lan truyền sự kiện đi nơi khác
    turnSearchBar(); // Gọi hàm để hiển thị hoặc ẩn thanh tìm kiếm
});

document.body.addEventListener('click', function(event) {
    if (searchBar.style.display === 'block' && !searchBar.contains(event.target)) {
        turnSearchBar(); // Nếu thanh tìm kiếm đang hiển thị và không có sự kiện click nào xảy ra trong thanh tìm kiếm, thì ẩn nó
    }
});

function turnSearchBar() {
    if (searchBar.style.display === 'block') {
        searchBar.style.display = 'none'; // Nếu đang hiển thị, ẩn đi
    } else {
        searchBar.style.display = 'block'; // Nếu đang ẩn, hiển thị lên
    }
}


//login

// document.getElementById("loginForm").addEventListener("submit", function(event) {
//     event.preventDefault(); // Ngăn chặn gửi form mặc định

//     var username = document.getElementById("username").value;
//     var password = document.getElementById("password").value;

//     var data = {
//         username: username,
//         password: password
//     };

//     // Sử dụng AJAX để gửi yêu cầu đăng nhập
//     var xhr = new XMLHttpRequest();
//     xhr.open("POST", "/Account/Login", true); // Điều chỉnh URL dựa trên địa chỉ máy chủ và phương thức xác thực
//     xhr.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
//     xhr.onreadystatechange = function() {
//         if (xhr.readyState === 4 && xhr.status === 200) {
//             var response = JSON.parse(xhr.responseText);
//             if (response.success) {
//                 alert("Đăng nhập thành công!");
//                 window.location.href = "../main.html";
//             } else {
//                 alert("Sai tài khoản hoặc mật khẩu.");
//             }
//         }
//     };
//     xhr.send(JSON.stringify(data)); // Gửi dữ liệu JSON đến máy chủ
// });


//product hiển thị giá
// function formatCurrency(number) {
//     var reverse = String(number).split('').reverse().join('');
//     var result = [];

//     for (var i = 0; i < reverse.length; i++) {
//         if (i % 3 === 0 && i !== 0) {
//             result.push('.');
//         }
//         result.push(reverse[i]);
//     }

//     var number = result.reverse().join('');
//     return number + ' đ';
// }

//đăng ký
// document.querySelector('form').addEventListener('submit', function(event) {
//     event.preventDefault(); // Ngăn chặn hành vi mặc định của form (tránh gửi yêu cầu tải lại trang)

//     // Lấy dữ liệu từ các trường input
//     var email = document.getElementById('email').value;
//     var password = document.getElementById('password').value;
//     var name = document.getElementById('name').value;
//     var address = document.getElementById('address').value;
//     var phone = document.getElementById('phone').value;
//     var birthday = document.getElementById('birthday').value;

//     // Tạo một đối tượng chứa dữ liệu đăng ký
//     var signupData = {
//         email: email,
//         password: password,
//         name: name,
//         address: address,
//         phone: phone,
//         birthday: birthday
//     };

//     // Gửi yêu cầu đăng ký tới máy chủ bằng AJAX
//     var xhr = new XMLHttpRequest();
//     xhr.open('POST', '/Account/Register', true);
//     xhr.setRequestHeader('Content-Type', 'application/json');
//     xhr.onload = function() {
//         if (xhr.status === 200) {
//             // Xử lý phản hồi từ máy chủ
//             var response = JSON.parse(xhr.responseText);
//             if (response.success) {
//                 alert('Đăng ký thành công!');
//                 // Chuyển hướng người dùng đến trang khác sau khi đăng ký thành công
//                 window.location.href = '../login.html';
//             } else {
//                 alert('Đăng ký thất bại: ' + response.message);
//             }
//         } else {
//             alert('Đã xảy ra lỗi: ' + xhr.status);
//         }
//     };
//     xhr.onerror = function() {
//         alert('Đã xảy ra lỗi kết nối.');
//     };
//     xhr.send(JSON.stringify(signupData));
// });

function generateVariants() {
    const colorsInput = document.getElementById('addColors').value.split(',').map(color => color.trim());
    const sizesInput = document.getElementById('addSizes').value.split(',').map(size => size.trim());
    const variantsTableContainer = document.getElementById('variantsTableContainer');
    variantsTableContainer.innerHTML = '';

    const table = document.createElement('table');
    const thead = document.createElement('thead');
    const tbody = document.createElement('tbody');

    const colorsCount = colorsInput.length;
    const sizesCount = sizesInput.length;

    // Tạo hàng tiêu đề
    const headerRow = document.createElement('tr');
    const headerCells = ['Màu sắc', 'Kích cỡ', 'Số lượng', 'Ảnh'].map(headerText => {
        const th = document.createElement('th');
        th.textContent = headerText;
        return th;
    });
    headerCells.forEach(cell => headerRow.appendChild(cell));
    thead.appendChild(headerRow);
    table.appendChild(thead);

    // Tạo các dòng dữ liệu
    for (let i = 0; i < colorsCount; i++) {
        for (let j = 0; j < sizesCount; j++) {
            const row = document.createElement('tr');
            const cells = [
                document.createElement('td'),
                document.createElement('td'),
                document.createElement('td'),
                document.createElement('td')
            ];

            cells[0].textContent = colorsInput[i];
            cells[1].textContent = sizesInput[j];
            cells[2].innerHTML = '<input type="number" name="quantities[]" min="0" value="0">';
            cells[3].innerHTML = '<input type="file" name="images[]" accept="image/*">';

            cells.forEach(cell => row.appendChild(cell));
            tbody.appendChild(row);
        }
    }

    table.appendChild(tbody);
    variantsTableContainer.appendChild(table);
}
function getParameterByName(name) {
    const urlParams = new URLSearchParams(window.location.search);
    return urlParams.get(name);
}

document.addEventListener('DOMContentLoaded', function () {
    const type = getParameterByName('type');
    if (type) {
        const radioButton = document.querySelector(`input[name="btnradio"][value="${type}"]`);
        if (radioButton) {
            radioButton.checked = true;
        }
    }
});