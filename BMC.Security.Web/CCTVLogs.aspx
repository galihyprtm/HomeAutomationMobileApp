<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CCTVLogs.aspx.cs" Inherits="BMC.Security.Web.CCTVLogs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
      <asp:GridView AutoGenerateColumns="False" CssClass="table table-bordered table-hover" ID="GvData" runat="server">
            <Columns>
                
                <asp:BoundField DataField="CCTVName" HeaderText="CCTV ID" SortExpression="CCTVName" />
                
                <asp:ImageField DataImageUrlField="ImageUrl" HeaderText="CCTV">
                </asp:ImageField>
                
                <asp:BoundField DataField="Tanggal" HeaderText="Tanggal" SortExpression="Tanggal" />
                <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                
            </Columns>
        </asp:GridView>
     <script>
        $(document).ready(function () {
            $('#<%= GvData.ClientID %>').DataTable();
        });
    </script>
</asp:Content>
