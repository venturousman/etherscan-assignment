<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="EtherscanAssignment._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link href="Styles/Default.css" rel="stylesheet" />
    <%--<div class="container"></div>--%>
    <div class="row">
        <div class="col-md-6">
            <h2>Save / Update Token</h2>
            <div class="form-horizontal">
                <input type="text" id="inputId" style="display: none">

                <div class="form-group">
                    <label for="inputName" class="col-sm-3 control-label">Name</label>
                    <div class="col-sm-9">
                        <input type="text" class="form-control" id="inputName" placeholder="Name" onchange="javascript:onInputChange(this)">
                        <span id="helperName" class="helper-text"></span>
                    </div>
                </div>
                <div class="form-group">
                    <label for="inputSymbol" class="col-sm-3 control-label">Symbol</label>
                    <div class="col-sm-9">
                        <input type="text" class="form-control" id="inputSymbol" placeholder="Symbol" onchange="javascript:onInputChange(this)">
                        <span id="helperSymbol" class="helper-text"></span>
                    </div>
                </div>
                <div class="form-group">
                    <label for="inputContractAddress" class="col-sm-3 control-label">Contract Address</label>
                    <div class="col-sm-9">
                        <input type="text" class="form-control" id="inputContractAddress" placeholder="Contract Address" onchange="javascript:onInputChange(this)">
                        <span id="helperContractAddress" class="helper-text"></span>
                    </div>
                </div>
                <div class="form-group">
                    <label for="inputTotalSupply" class="col-sm-3 control-label">Total Supply</label>
                    <div class="col-sm-9">
                        <input type="number" class="form-control" id="inputTotalSupply" placeholder="Total Supply" onchange="javascript:onInputChange(this)">
                        <span id="helperTotalSupply" class="helper-text"></span>
                    </div>
                </div>
                <div class="form-group">
                    <label for="inputTotalHolders" class="col-sm-3 control-label">Total Holders</label>
                    <div class="col-sm-9">
                        <input type="number" class="form-control" id="inputTotalHolders" placeholder="Total Holders" onchange="javascript:onInputChange(this)">
                        <span id="helperTotalHolders" class="helper-text"></span>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-offset-3 col-sm-9">
                        <button type="button" class="btn btn-primary" onclick="javascript:submitForm()">Save</button>
                        <button type="button" class="btn btn-default" onclick="javascript:resetForm()">Reset</button>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="chart-container">
                <canvas id="myChart"></canvas>
            </div>
        </div>
    </div>
    <hr class="divider" />
    <div class="row">
        <div class="col-md-12">
            <button type="button" class="btn btn-default" onclick="javascript:tableToCSV()">Export</button>
        </div>
    </div>
    <div class="table-responsive">
        <table class="table table-bordered">
            <thead>
                <tr>
                    <th>Rank</th>
                    <th>Symbol</th>
                    <th>Name</th>
                    <th>Contract Address</th>
                    <th>Total Holders</th>
                    <th>Total Supply</th>
                    <th>Total Supply %</th>
                    <th></th>
                </tr>
            </thead>
            <tbody id="tbl-body-tokens">
            </tbody>
        </table>
    </div>
    <div class="row">
        <div class="col-md-12">
            <nav aria-label="Page navigation">
                <ul class="pagination" id="pagination">
                    <li>
                        <a href="#" aria-label="Previous">
                            <span aria-hidden="true">&laquo;</span>
                        </a>
                    </li>
                    <li><a href="#">1</a></li>
                    <li><a href="#">2</a></li>
                    <li><a href="#">3</a></li>
                    <li><a href="#">4</a></li>
                    <li><a href="#">5</a></li>
                    <li>
                        <a href="#" aria-label="Next">
                            <span aria-hidden="true">&raquo;</span>
                        </a>
                    </li>
                </ul>
            </nav>
        </div>
    </div>
    <script src="Scripts/App/Default.js"></script>
    <script src="Scripts/App/ExportToCSV.js"></script>
</asp:Content>
