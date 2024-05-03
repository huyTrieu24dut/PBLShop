
document.getElementById("loginForm").addEventListener("submit", function(event) {
    event.preventDefault(); // Ngăn chặn gửi form mặc định

    var username = document.getElementById("username").value;
    var password = document.getElementById("password").value;

    var data = {
        username: username,
        password: password
    };

    // Sử dụng AJAX để gửi yêu cầu đăng nhập
    var xhr = new XMLHttpRequest();
    xhr.open("POST", "/Account/Login", true); // Điều chỉnh URL dựa trên địa chỉ máy chủ và phương thức xác thực
    xhr.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    xhr.onreadystatechange = function() {
        if (xhr.readyState === 4 && xhr.status === 200) {
            var response = JSON.parse(xhr.responseText);
            if (response.success) {
                alert("Đăng nhập thành công!");
                window.location.href = "../main.html";
            } else {
                alert("Sai tài khoản hoặc mật khẩu.");
            }
        }
    };
    xhr.send(JSON.stringify(data)); // Gửi dữ liệu JSON đến máy chủ
});
