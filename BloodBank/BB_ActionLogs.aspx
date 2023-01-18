<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BB_ActionLogs.aspx.cs" Inherits="BloodBank.BB_ActionLogs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, shrink-to-fit=no" />
    <title>BloodBank Activity Logs</title>
    <link rel="stylesheet" href="assets/bootstrap/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Nunito:200,200i,300,300i,400,400i,600,600i,700,700i,800,800i,900,900i&amp;display=swap" />
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Alegreya+Sans" />
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Black+Han+Sans" />
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Capriola" />
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Francois+One" />
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Gafata" />
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Gayathri" />
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Gothic+A1" />
    <link rel="stylesheet" href="assets/fonts/fontawesome-all.min.css" />
    <link rel="stylesheet" href="assets/fonts/font-awesome.min.css" />
    <link rel="stylesheet" href="assets/fonts/fontawesome5-overrides.min.css" />
    <link rel="stylesheet" href="assets/css/Google-Style-Login.css" />
    <link rel="stylesheet" href="assets/css/login-full-page-bs4.css" />
    <link rel="stylesheet" href="assets/css/Modal-Login-form.css" />
    <link rel="stylesheet" href="assets/css/Table-With-Search-1.css" />
    <link rel="stylesheet" href="assets/css/Table-With-Search.css" />
</head>
<body id="page-top">
    <form runat="server" id="wrapper">
        <nav class="navbar navbar-dark align-items-start sidebar sidebar-dark accordion bg-gradient-primary p-0" style="background: rgb(119,40,32);">
            <div class="container-fluid d-flex flex-column p-0">
                <img style="font-size: 12px; line-height: 23px; border-width: -14px; height: 97px;" src="assets/img/vsmmclogo1.png"><a class="navbar-brand d-flex justify-content-center align-items-center sidebar-brand m-0" href="#">
                    <div class="sidebar-brand-icon rotate-n-15"></div>
                    <div class="sidebar-brand-text mx-3"><span style="font-size: 30px;">VSMMC</span></div>
                </a>
                <hr class="sidebar-divider my-0">
                <ul class="navbar-nav text-light" id="accordionSidebar">
                    <li class="nav-item"><a class="nav-link" href="BB_Dashboard.aspx"><i class="fas fa-tachometer-alt" style="font-size: 20px;"></i><span style="font-size: 15px;">Dashboard</span></a></li>
                   
                    <li class="nav-item"><a class="nav-link" href="bloodtransaction.html"><i class="fa fa-tint" style="font-size: 20px;"></i><span style="font-size: 15px;">Blood Transaction</span></a></li>
                    <li class="nav-item"><a class="nav-link active" href="BB_ActionLogs.aspx"><i class="fa fa-list-ul" style="font-size: 20px;"></i><span style="font-size: 15px;">Action Log</span></a></li>
                </ul>
                <div class="text-center d-none d-md-inline"></div>
            </div>
        </nav>
        <div class="d-flex flex-column" id="content-wrapper">
            <div id="content" style="background: linear-gradient(rgb(249,243,243) 28%, white), #ffffff;">
                <nav class="navbar navbar-light navbar-expand bg-white shadow mb-4 topbar static-top">
                    <div class="container-fluid">
                        <button class="btn btn-link d-md-none rounded-circle mr-3" id="sidebarToggleTop" type="button"><i class="fas fa-bars"></i></button>
                        <div class="form-inline d-none d-sm-inline-block mr-auto ml-md-3 my-2 my-md-0 mw-100 navbar-search">
                            <input class="form-control-plaintext" type="text" value="Dashboard" readonly="" style="font-size: 40px;"></div>
                        <ul class="navbar-nav flex-nowrap ml-auto">
                            <li class="nav-item dropdown no-arrow mx-1">
                                <div class="nav-item dropdown no-arrow">
                                    <a class="dropdown-toggle nav-link" aria-expanded="false" data-toggle="dropdown" href="#"><span class="badge badge-danger badge-counter">3+</span><i class="fas fa-bell fa-fw" style="font-size: 29px; color: rgb(119,40,32);"></i></a>
                                    <div class="dropdown-menu dropdown-menu-right dropdown-list animated--grow-in">
                                        <h6 class="dropdown-header">alerts center</h6>
                                        <a class="dropdown-item d-flex align-items-center" href="#">
                                            <div class="mr-3">
                                                <div class="bg-primary icon-circle"><i class="fas fa-tint text-white"></i></div>
                                            </div>
                                            <div>
                                                <span class="small text-gray-500">December 12, 2019</span>
                                                <p>New Blood Request</p>
                                            </div>
                                        </a><a class="dropdown-item text-center small text-gray-500" href="#">Show All Alerts</a>
                                    </div>
                                </div>
                            </li>
                            <div class="d-none d-sm-block topbar-divider"></div>
                            <li class="nav-item dropdown no-arrow">
                                <div class="nav-item dropdown show no-arrow">
                                    <a class="dropdown-toggle nav-link" aria-expanded="true" data-toggle="dropdown" href="#"><span class="d-none d-lg-inline mr-2 text-gray-600 small" runat="server" id="username"></span>
                                        <img class="border rounded-circle img-profile" src="assets/img/user.png" /></a>
                                    <div class="dropdown-menu show shadow dropdown-menu-right animated--grow-in">
                                        <a class="dropdown-item" href="BB_Profile.aspx"><i class="fas fa-user fa-sm fa-fw mr-2 text-gray-400"></i>&nbsp;Profile</a>
                                        <div class="dropdown-divider"></div>
                                        <a class="dropdown-item" href="#"><i class="fas fa-sign-out-alt fa-sm fa-fw mr-2 text-gray-400"></i>&nbsp;Logout</a>
                                    </div>
                                </div>
                            </li>
                        </ul>
                    </div>
                </nav>
                <div class="container-fluid">
                    <div class="row">
                        <div class="col">
                            <div class="card shadow">
                                <div class="card-header py-3">
                                    <h1>BloodBank Activity Logs</h1>
                                </div>
                                <div class="card-body">
                                    <h3 runat="server" id="NoDataMsg" style="display: none;">No Data</h3>
                                    <div runat="server" id="TableContainer" style="max-height: 450px">
                                        <div id="VerticalScroll" style="overflow: auto; max-height: inherit;">
                                            <asp:GridView runat="server" ID="ActionLogs" Visible="true" AutoGenerateColumns="false" Width="100%"
                                                BorderColor="Transparent" AutoPostBack="false">
                                                <RowStyle CssClass="grid-item-style  grid-font-style" />
                                                <Columns>
                                                    <asp:BoundField HeaderText="ID" DataField="BL_ID" />
                                                    <asp:BoundField HeaderText="DESCRIPTION" DataField="BL_DESCRIPTION" />
                                                    <asp:BoundField HeaderText="USER ID" DataField="BL_BB_ID" />
                                                    <asp:BoundField HeaderText="DATE" DataField="BL_DATE" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <a class="border rounded d-inline scroll-to-top" href="#page-top"><i class="fas fa-angle-up"></i></a>
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
