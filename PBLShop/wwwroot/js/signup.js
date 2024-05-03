document.querySelector('form').addEventListener('submit', function(event) {
    event.preventDefault(); // Ngăn chặn hành vi mặc định của form (tránh gửi yêu cầu tải lại trang)

    // Lấy dữ liệu từ các trường input
    var email = document.getElementById('email').value;
    var password = document.getElementById('password').value;
    var name = document.getElementById('name').value;
    var address = document.getElementById('address').value;
    var phone = document.getElementById('phone').value;
    var birthday = document.getElementById('birthday').value;

    // Tạo một đối tượng chứa dữ liệu đăng ký
    var signupData = {
        email: email,
        password: password,
        name: name,
        address: address,
        phone: phone,
        birthday: birthday
    };

    // Gửi yêu cầu đăng ký tới máy chủ bằng AJAX
    var xhr = new XMLHttpRequest();
    xhr.open('POST', '/Account/Register', true);
    xhr.setRequestHeader('Content-Type', 'application/json');
    xhr.onload = function() {
        if (xhr.status === 200) {
            // Xử lý phản hồi từ máy chủ
            var response = JSON.parse(xhr.responseText);
            if (response.success) {
                alert('Đăng ký thành công!');
                // Chuyển hướng người dùng đến trang khác sau khi đăng ký thành công
                window.location.href = '../login.html';
            } else {
                alert('Đăng ký thất bại: ' + response.message);
            }
        } else {
            alert('Đã xảy ra lỗi: ' + xhr.status);
        }
    };
    xhr.onerror = function() {
        alert('Đã xảy ra lỗi kết nối.');
    };
    xhr.send(JSON.stringify(signupData));
});
