﻿<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
     <script type="text/javascript">
         $(document).ready(function () {
             $('#admin').addClass("nav-current");
             $('#admin-munki').addClass("nav-current-sub");
         });
        </script>
    <h1>Admin->Munki</h1>
   
</asp:Content>

