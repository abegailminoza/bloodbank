<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BB_Notification.aspx.cs" Inherits="BloodBank.BB_Notification" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="icon" runat="server" href="~/assets/img/321479999_548324667206662_5830804446592810955_n.png" />
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, shrink-to-fit=no" />
    <title>Notification</title>
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
                    <li class="nav-item"><a class="nav-link active" href="BB_Dashboard.aspx"><i class="fas fa-tachometer-alt" style="font-size: 20px;"></i><span style="font-size: 15px;">Dashboard</span></a></li>
                    <li class="nav-item"><a class="nav-link" href="BB_BloodTransaction.aspx"><i class="fa fa-tint" style="font-size: 20px;"></i><span style="font-size: 15px;">Blood Transaction</span></a></li>
                    <li class="nav-item"><a class="nav-link" href="BB_ActionLogs.aspx"><i class="fa fa-list-ul" style="font-size: 20px;"></i><span style="font-size: 15px;">Action Logs</span></a></li>
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
                            <input class="form-control-plaintext" type="text" value="Dashboard" readonly="" style="font-size: 40px;">
                        </div>
                        <ul class="navbar-nav flex-nowrap ml-auto">
                            <li class="nav-item dropdown show no-arrow mx-1">
                                <div class="nav-item dropdown no-arrow">
                                    <a class="dropdown-toggle nav-link" aria-expanded="false" data-bs-toggle="dropdown" href="#"><span class="badge bg-danger badge-counter" runat="server" id="UnreadCount"></span><i class="fas fa-bell fa-fw"></i></a>
                                    <div class="dropdown-menu dropdown-menu-end dropdown-list animated--grow-in">
                                        <h6 class="dropdown-header" style="background: rgb(119,40,32);">NOTIFICATIONS</h6>
                                        <div class="d-flex" style="flex-direction: column; max-height: 250px; overflow: auto; width: 100%;">
                                            <asp:Repeater runat="server" ID="NotificationNavList">
                                                <ItemTemplate>
                                                    <a class="dropdown-item d-flex align-items-center" href="BB_Notification.aspx">
                                                        <div class="me-3">
                                                            <div class="bg-primary icon-circle" style="background: var(--bs-indigo); border-color: var(--bs-blue);"><i class="fas fa-envelope-open text-white"></i></div>
                                                        </div>
                                                        <div>
                                                            <span class="small text-gray-500"><%# Eval("NTF_DATE") %></span>
                                                            <p><%# Eval("NTF_SUBJECT") %></p>
                                                        </div>
                                                    </a>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                        <a class="dropdown-item text-center small text-gray-500" href="BB_Notification.aspx">Show All Notifications</a>
                                    </div>
                                </div>
                            </li>
                            <div class="d-none d-sm-block topbar-divider"></div>
                            <li class="nav-item dropdown no-arrow">

                                <div class="nav-item dropdown no-arrow">
                                    <a class="dropdown-toggle nav-link" data-toggle="dropdown" aria-expanded="false" href="#"><span class="d-none d-lg-inline mr-2 text-gray-600 small" runat="server" id="username"></span>
                                        <img class="border rounded-circle img-profile" src="assets/img/user.png" /></a>
                                    <div class="dropdown-menu show shadow dropdown-menu-right animated--grow-in">
                                        <a class="dropdown-item" href="BB_Profile.aspx"><i class="fas fa-user fa-sm fa-fw mr-2 text-gray-400"></i>&nbsp;Profile</a>
                                        <div class="dropdown-divider"></div>
                                        <a class="dropdown-item" runat="server" id="BtnLogout" autopostback="true" onserverclick="BtnLogout_ServerClick"><i class="fas fa-sign-out-alt fa-sm fa-fw mr-2 text-gray-400"></i>&nbsp;Logout</a>

                                    </div>
                                </div>
                            </li>
                        </ul>
                    </div>
                </nav>
                <div class="container-fluid">
                    <div class="row" style="margin-left: -116px;padding-left: 118px;">
                        <div class="col-md-12 col-xxl-5 search-table-col"><span class="counter pull-right"></span>
                            <div class="table-responsive table table-hover table-bordered results">
                                <table class="table table-hover table-bordered">
                                    <thead class="bill-header cs">
                                        <tr>
                                            <th id="trs-hd-2" class="col-lg-2" style="width: 124.7188px;">Id</th>
                                            <th id="trs-hd-3" class="col-lg-3" style="width: 169.078px;">Subject</th>
                                            <th id="trs-hd-4" class="col-lg-2" style="width: 131.7188px;">Date</th>
                                            <th id="trs-hd-6" class="col-lg-2" style="width: 119.7188px;">Status</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr class="warning no-result">
                                            <td colspan="12"><i class="fa fa-warning"></i>&nbsp; No Result !!!</td>
                                        </tr>
                                        <tr>
                                            <td>01</td>
                                            <td>India</td>
                                            <td>Souvik Kundu</td>
                                            <td>2014</td>
                                            <td><button class="btn btn-success" style="margin-left: 5px;background: rgb(119,40,32);" type="submit"><i class="far fa-paper-plane" style="font-size: 15px;border-color: rgb(244,237,237);color: rgb(247,241,241);"></i></button></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                        <div class="col-md-6 col-xxl-5">
                            <div class="card" style="width: 80rem;border-top-left-radius: 20px;border-top-right-radius: 20px;border-bottom-right-radius: 20px;border-bottom-left-radius: 20px;box-shadow: 5px 5px 16px 2px rgba(0,0,0,0.25);margin: 28px;min-width: 280px;max-width: 300px;margin-bottom: 20px;height: 443px;">
                                <div class="card-body d-flex flex-column" style="height: 434px;width: 98%;">
                                    <div>
                                        <h4 style="font-family: 'Source Sans Pro', sans-serif;font-weight: 700;color: rgb(255,160,0);">Notification</h4>
                                        <h6 class="text-muted mb-2" style="font-family: 'Source Sans Pro', sans-serif;font-weight: 600;color: #757575;">Subject</h6>
                                    </div><input type="text">
                                    <p>Message</p><textarea style="height: 256px;width: 251px;"></textarea>
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
    <script src="assets/js/chart.min.js"></script>
    <script src="assets/js/bs-init.js"></script>
    <script src="assets/js/login-full-page-bs4.js"></script>
    <script src="assets/js/login-full-page-bs4-1.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-easing/1.4.1/jquery.easing.js"></script>
    <script src="assets/js/Table-With-Search.js"></script>
    <script src="assets/js/theme.js"></script>
</body>

</html>
