<%@ Page Async="true" Title="BMC Security 0.1" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BMC.Security.Web._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-4">
            <asp:Panel ID="PassPanel" runat="server">
                <table class="table">
                    <tr>
                        <td colspan="3">Welcome to BMC Security Panel 0.1
                        </td>
                    </tr>
                    <tr>
                        <td>PASSCODE</td>
                        <td>:</td>
                        <td>
                            <asp:TextBox TextMode="Password" ID="TxtPass" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Button CssClass="btn btn-info" ID="BtnPass" runat="server" Text="Enter" /></td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Label ForeColor="Red" Font-Bold="true" runat="server" ID="TxtLogin"></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="ControlPanel" Visible="false" runat="server">
                <table class="table">
                    <tr>
                        <td colspan="3">BMC Security Control 0.1</td>
                    </tr>
                    <tr>
                        <td>No</td>
                        <td>Description</td>
                        <td>Action</td>
                    </tr>
                    <tr>
                        <td>1</td>
                        <td>Keluarkan Suara Monster</td>
                        <td>
                            <asp:Button CssClass="btn" CommandName="Monster" ID="BtnMonster" runat="server" Text="Play" /></td>
                    </tr>
                    <tr>
                        <td>2</td>
                        <td>Keluarkan Suara Tornado Sirine</td>
                        <td>
                            <asp:Button CssClass="btn" CommandName="Tornado" ID="BtnTornado" runat="server" Text="Play" /></td>
                    </tr>
                    <tr>
                        <td>3</td>
                        <td>Keluarkan Suara Polisi</td>
                        <td>
                            <asp:Button CssClass="btn" CommandName="Police" ID="BtnPolice" runat="server" Text="Play" /></td>
                    </tr>
                    <tr>
                        <td>4</td>
                        <td>Keluarkan Suara Teriakan</td>
                        <td>
                            <asp:Button CssClass="btn" CommandName="Scream" ID="BtnScream" runat="server" Text="Play" /></td>
                    </tr>
                    <tr>
                        <td>5</td>
                        <td>Nyalakan LED</td>
                        <td>
                            <asp:Button CssClass="btn" CommandName="LEDON" ID="BtnLedOn" runat="server" Text="Turn On" /></td>
                    </tr>
                    <tr>
                        <td>6</td>
                        <td>Matikan LED</td>
                        <td>
                            <asp:Button CssClass="btn" CommandName="LEDOFF" ID="BtnLedOff" runat="server" Text="Turn Off" /></td>
                    </tr>
                     <tr>
                        <td>7</td>
                        <td>CCTV Watcher Status</td>
                        <td>
                            <asp:Button CssClass="btn" CommandName="CCTVStatus" CommandArgument="True" ID="BtnCCTVOn" runat="server" Text="Turn On" />
                            <asp:Button CssClass="btn" CommandName="CCTVStatus" CommandArgument="False" ID="BtnCCTVOff" runat="server" Text="Turn Off" /></td>
                    </tr>
                    <tr>
                        <td>8</td>
                        <td>CCTV Watcher Update Time</td>
                        <td>
                            Interval : <asp:TextBox ID="TxtInterval" TextMode="Number"  runat="server"></asp:TextBox> Detik
                            <asp:Button CssClass="btn" CommandName="CCTVUpdateTime" ID="BtnCCTVInterval" runat="server" Text="Set Waktu" /></td>
                    </tr>
                </table>
                <table class="table table-bordered table-hovered">
                 <tr>
                    <td>No</td>
                    <td>Device Name</td>
                    <td>
                       Actions
                    </td>
                </tr>
                    <asp:Repeater runat="server" ID="RptControlDevice"> 
                        <ItemTemplate>
                             <tr>
                                <td><%# Eval("No") %></td>
                                <td>Device <%# Eval("No") %></td>
                                <td>
                                    <asp:Button CssClass="btn btn-info" OnClick="DoAction" CommandName="DEVICEON" CommandArgument='<%#Eval("No") %>' runat="server" Text="On" />
                                    <asp:Button CssClass="btn btn-danger" OnClick="DoAction" CommandName="DEVICEOFF" CommandArgument='<%#Eval("No") %>' runat="server" Text="Off" />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
               
                 
                </table>
                <div class="well">
                    <asp:Label ID="TxtStatus" runat="server" Text=""></asp:Label>
                </div>
            </asp:Panel>

        </div>
    </div>

</asp:Content>
