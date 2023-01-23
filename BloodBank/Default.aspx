<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BloodBank._Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="icon" runat="server" href="~/assets/img/321479999_548324667206662_5830804446592810955_n.png" />
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, shrink-to-fit=no" />
    <title>Login | Blood Bank</title>
    <link rel="stylesheet" href="assets/bootstrap/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Nunito:200,200i,300,300i,400,400i,600,600i,700,700i,800,800i,900,900i&amp;display=swap" />
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Alegreya+Sans" />
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Black+Han+Sans" />
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Capriola" />
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Francois+One" />
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Gafata" />
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Gayathri" /> 
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Gothic+A1" />
    <link rel="stylesheet" href="assets/css/Google-Style-Login.css" />
    <link rel="stylesheet" href="assets/css/login-full-page-bs4.css" />
    <link rel="stylesheet" href="assets/css/Modal-Login-form.css" />
    <link rel="stylesheet" href="assets/css/Table-With-Search-1.css" />
    <link rel="stylesheet" href="assets/css/Table-With-Search.css" />
</head>

<body class="d-flex justify-content-xl-center align-items-xl-center bg-gradient-primary" style="background: rgb(255,255,255);height: 100vh;">
    <form runat="server" id="Main" class="container">
        <div class="row justify-content-center">
            <div class="col-md-9 col-lg-12 col-xl-10">
                <div class="card shadow-lg o-hidden border-0 my-5" style="height: fit-content;">
                    <div class="card-body p-0">
                        <div class="row">
                            <div class="col-lg-6 d-flex d-lg-flex justify-content-xl-center align-items-xl-center"><img src="assets/img/vsmmclogo1.png" style="width: 364px;" />
                            </div>
                            <div class="col-lg-6 d-flex justify-content-xl-center align-items-xl-center">
                                <div class="p-5" style="width: 100%;">
                                    <div class="text-center" style="margin-bottom: 15px;">
                                        <h4 class="text-dark mb-4">VSMMC Blood Bank</h4>
                                    </div>
                                    <hr style="margin-bottom: 24px;">
                                    <div class="user">
                                        <div class="form-group" style="margin-bottom: 19px;">
                                            <asp:TextBox runat="server" Class="form-control d-flex justify-content-xl-center align-items-xl-center form-control-user" ID="Username" placeholder="Username" autocomplete="on" required="" />
                                        </div>
                                        <div class="form-group" style="margin-bottom: 24px;">
                                            <asp:TextBox runat="server" Class="form-control form-control-user" type="password" ID="Password" placeholder="Password" name="password" required="" />
                                        </div>
                                        <hr />
                                        <asp:Button runat="server" Class="btn btn-primary btn-block btn-lg btn-signin" OnClick="Login_Click" ID="Login" type="Confirm" style="background: rgb(119,40,32);" Text="LOGIN" />
                                        <hr />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script src="assets/js/jquery.min.js"></script>
    <script src="assets/bootstrap/js/bootstrap.min.js"></script>
    <script src="assets/js/login-full-page-bs4.js"></script>
    <script src="assets/js/login-full-page-bs4-1.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-easing/1.4.1/jquery.easing.js"></script>
    <script src="assets/js/Table-With-Search.js"></script>
    <script src="assets/js/theme.js"></script>
</body>

</html>