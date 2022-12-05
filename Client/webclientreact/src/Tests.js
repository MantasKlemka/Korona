import React, { useEffect, useRef, useState} from "react";
import { Navigate, useNavigate } from "react-router-dom";
import { Button } from 'react-bootstrap';
import {ReactComponent as Logo} from './medication.svg';

export default function Tests() {
    const navigate = useNavigate();
    const [tableRows, setTableRows] = useState([]);

    const [date, setDate] = useState([]);
    const [type, setType] = useState([]);
    const [result, setResult] = useState([]);
    const [isolation, setIsolation] = useState([]);

    const [visibilityStatus, setVisibilityStatus] = useState([]);
    const [isolationID, setIsolationID] = useState([]);
    const [testID, setTestID] = useState([]);
    const [idEdit, setIdEdit] = useState([]);
    const [dateEdit, setDateEdit] = useState([]);
    const [typeEdit, setTypeEdit] = useState([]);
    const [resultEdit, setResultEdit] = useState([]);
    const [isolationEdit, setIsolationEdit] = useState([]);
    const [idToDelete, setIdToDelete] = useState([]);
    const [loading, setLoading] = useState([]);


    useEffect(() => {
        setLoading(false);
        setVisibilityStatus("hidden");
        if(sessionStorage.getItem("token") === null) {
            navigate("/");
        }
    }, []);

    function saveEdit(){
        setLoading(true);
        let json = {};
        if(dateEdit != date){
            json["date"] = date;
        }
        if(typeEdit != type){
            json["type"] = type;
        }
        if(resultEdit != result){
            json["result"] = result;
        }
        if(isolationEdit != isolation){
            json["isolation"] = isolation;
        }
        if(JSON.stringify(json) != '{}')
        {
            json = JSON.stringify(json)
            fetchEdit(json);
        }
        else{
            setLoading(false);
        }
    }

    function fetchCreate(json){
        const requestOptions = {
            method: 'POST',
            headers: { 
                'Authorization':'Bearer ' + sessionStorage.getItem("token"),
                'Content-Type': 'application/json'},
            body : json
        };
        fetch('https://korona.azurewebsites.net/api/Test/', requestOptions)
        .then(res => afterFetchCreate(res))
    }

    function fetchEdit(json){
        const requestOptions = {
            method: 'PUT',
            headers: { 
                'Authorization':'Bearer ' + sessionStorage.getItem("token"),
                'Content-Type': 'application/json'},
            body : json
        };
        fetch('https://korona.azurewebsites.net/api/Test/' + idEdit, requestOptions)
        .then(res => afterFetchEdit(res))
    }

    function afterFetchCreate(res){
        if(res.status != 200)
        {
            let error = document.getElementById('errorCreate');
            res.text().then(result => error.textContent = result);
        }
        else
        {
            window.location.reload(true);
        }
        setLoading(false);
    }
    
    function afterFetchEdit(res){
        if(res.status != 200)
        {
            let error = document.getElementById('errorEdit');
            res.text().then(result => error.textContent = result);
        }
        else
        {
            window.location.reload(true);
        }
        setLoading(false);
    }

    async function fetchGetAllByIsolation(){
        const requestOptions = {
            method: 'GET',
            headers: { 
                'Authorization':'Bearer ' + sessionStorage.getItem("token"),
                'Content-Type': 'application/json'}
        };
        const res = await fetch('https://korona.azurewebsites.net/api/Isolation/' + isolationID + '/Tests', requestOptions)
        const data = await res.text();
        fillTable(res, data, "Isolation " + isolationID + " tests");
    }

    async function fetchGetAll(){
        const requestOptions = {
            method: 'GET',
            headers: { 
                'Authorization':'Bearer ' + sessionStorage.getItem("token"),
                'Content-Type': 'application/json'}
        };
        const res = await fetch('https://korona.azurewebsites.net/api/Test/All/', requestOptions)
        const data = await res.text();
        fillTable(res, data, "All Tests");
    }

    async function fetchDelete(){
        const requestOptions = {
            method: 'DELETE',
            headers: { 
                'Authorization':'Bearer ' + sessionStorage.getItem("token"),
                'Content-Type': 'application/json'}
        };
        const res = await fetch('https://korona.azurewebsites.net/api/Test/' + idToDelete, requestOptions);
        const data = await res.text();
        afterFetchDelete(res, data);
    }

    function afterFetchDelete(res, data){
        if(res.status != 200)
        {
            let error = document.getElementById('errorDelete');
            error.textContent = data;
        }
        else
        {
            window.location.reload(true);
        }
        setLoading(false);
    }

    async function fetchGetById(){
        const requestOptions = {
            method: 'GET',
            headers: { 
                'Authorization':'Bearer ' + sessionStorage.getItem("token"),
                'Content-Type': 'application/json'}
        };
        const res = await fetch('https://korona.azurewebsites.net/api/Test/' + testID, requestOptions);
        const data = await res.text();
        fillTable(res, data, "Test " + testID);
    }

    function fillTable(res, answer, title){
        if(res.status != 200)
        {
            let error = document.getElementById('errorLoad');
            error.textContent = answer;
        }
        else{
            document.getElementById('tableTitleText').textContent = title;
            var tests = JSON.parse(answer);
            let i = 0;
            if(tests.constructor == Array){
                tests.map(test=>
                (
                    createRow(test, i),
                    i++
                ));
            }
            else{
                createRow(tests, i)
            }
            setVisibilityStatus("visible");
        }
        setLoading(false);

    }

    function createRow(test, i){
        let dateText = test.date.split("/");
        dateText = dateText[2] + "-" + (dateText[0] > 9 ? "" + dateText[0] : "0" + dateText[0]) + "-" + (dateText[1]> 9 ? "" + dateText[1] : "0" + dateText[1])
        let typeText = test.type;
        let resultText = test.result;
        let isolationText = test.isolation;
        let id = React.createElement('td',{key: "id" + i}, test.id);
        let type = React.createElement('td',{key: "type" + i}, typeText);
        let result = React.createElement('td',{key: "result" + i}, resultText);
        let date = React.createElement('td',{key: "date" + i}, dateText);
        let isolation = React.createElement('td',{key: "isolation" + i},isolationText);
        let deleteTest = React.createElement('td', {key: "delete" + i},"", React.createElement('button',{key: "deleteModal" + i, 'data-bs-toggle': "modal", 'data-bs-target': "#deleteModal", 'className': "btn floatButton", onClick: () => setIdToDelete(test.id) },"Delete"));
        let edit = React.createElement('td', {key: "edit" + i},"", React.createElement('button',{key: "editModal" + i, 'data-bs-toggle': "modal", 'data-bs-target': "#editModal", 'className': "btn floatButton", onClick: () => fillEdit(test.id, dateText, typeText, resultText, isolationText) },"Edit"));
        let childs = [id, date, type, result, isolation, edit, deleteTest]
        let element = React.createElement('tr', {key: "row" + i}, childs);
        setTableRows(oldArray => [oldArray, element]);
    }

    function DeleteTest(){
        fetchDelete();
    }

    function fillEdit(id, date, type, result, isolation){
        setDateEdit(date);
        setTypeEdit(type);
        setResultEdit(result);
        setIsolationEdit(isolation);
        setIdEdit(id);
        setDate(date);
        setType(type);
        setResult(result);
        setIsolation(isolation);
    }

    function loadAll(){
        setLoading(true);
        setVisibilityStatus("hidden");
        document.getElementById('errorLoad').textContent = "";
        document.getElementById('tableTitleText').textContent = "";
        setTableRows();
        fetchGetAll();
    }

    function loadByIsolation(){
        setLoading(true);
        setVisibilityStatus("hidden");
        document.getElementById('errorLoad').textContent = "";
        document.getElementById('tableTitleText').textContent = "";
        if(isolationID.length === 0){
            setLoading(false);
            return;
        }
        setTableRows();
        fetchGetAllByIsolation();
    }

    function loadById(){
        setLoading(true);
        setVisibilityStatus("hidden");
        document.getElementById('errorLoad').textContent = "";
        document.getElementById('tableTitleText').textContent = "";
        if(testID.length === 0){
            setLoading(false);
            return;
        }
        setTableRows();
        fetchGetById();
    }

    function create(){
        setLoading(true);
        if(date.length === 0 || type.length === 0 || result.length === 0 || isolation.length === 0){
            let error = document.getElementById('errorCreate');
            error.textContent = "All fields should be filled!"
        }
        else{
            let json = {};
            json["date"] = date;
            json["type"] = type;
            json["result"] = result;
            json["isolation"] = isolation;
    
            if(JSON.stringify(json) != '{}')
            {
                json = JSON.stringify(json)
                fetchCreate(json);
            }
            else{
                setLoading(false);
            }
        }
    }

    return (
        <>
            {loading ? (<div className="loaderDiv">
                <div className="spinner"></div>
            </div>) : ("")}
            <nav className="navbar fixed-top navbar-expand-md navBar">
                <a className="navbar-brand" href="/mainpage">
                    <Logo className='logoMedic'/>
                </a>
                <button className="navbar-toggler toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNavAltMarkup" aria-controls="navbarNavAltMarkup" aria-expanded="false" aria-label="Toggle navigation">
                    <span className="navbar-toggler-icon">
                        <img src="/menu.svg"></img>
                    </span>
                </button>
                <div className="collapse navbar-collapse" id="navbarNavAltMarkup">
                    <div className="navbar-nav">
                        <a className="nav-item nav-link" href="/Doctors">Doctors</a>
                        <a className="nav-item nav-link" href="/Pacients">Pacients</a>
                        <a className="nav-item nav-link"  href="/Isolations">Isolations</a>
                        <a className="nav-link nav-link" active>Tests</a>
                    </div>
                    <div className="navbar-nav">
                        <a className="nav-item nav-link" href="/">Logout</a>
                    </div>
                </div>
            </nav>
            <div className="infoTable">
                <div className="actionElements">
                <p id="actionsText"><b>Actions</b></p>
                    <p id="errorLoad" className="errorTextTitle"></p>
                    <input className="form-control inputs inputLoad" type="text" size="20" placeholder="Test ID" onChange = {(e) => {setTestID(e.target.value); }}/>
                    <Button className="btn-secondary loadButton" onClick={() => loadById()}>Load Test by Id</Button><br></br><br></br>                 
                    <input className="form-control inputs inputLoad" type="text" size="20" placeholder="Isolation ID" onChange = {(e) => {setIsolationID(e.target.value); }}/>
                    <Button className="btn-secondary loadButton" onClick={() => loadByIsolation()}>Load Isolation' Tests</Button><br></br><br></br>                
                    <Button className="btn-secondary loadButton" onClick={() => loadAll()}>Load All Tests</Button><br></br>
                    <Button className="btn-secondary loadButton" data-bs-toggle="modal" data-bs-target = "#createModal">Create new Test</Button>
                </div>
                
                <br></br><br></br><br></br><br></br>
                <b id="tableTitleText"></b>
                <table id="testsTable" className="table" style={{visibility: visibilityStatus}}>
                    <thead className="titleRow">
                        <tr>
                            <th scope="col">ID</th>
                            <th scope="col">Date</th>
                            <th scope="col">Type</th>
                            <th scope="col">Result</th>
                            <th scope="col">Isolation</th>
                            <th scope="col"></th>
                            <th scope="col"></th>
                        </tr>
                    </thead>
                    <tbody>
                        {tableRows}                       
                    </tbody>
                </table>
            </div>
            <div className="modal fade" id="editModal" tabIndex="-1" role="dialog" aria-labelledby="editModalLabel" aria-hidden="true">
                <div className="modal-dialog" role="document">
                    <div className="modal-content">
                        <div className="modal-header">
                            <h5 className="modal-title" id="editModalLabel">Edit</h5>
                            <button type="button" className="btn floatButton" data-bs-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div className="modal-body">
                            <p id="errorEdit" className="errorTextTitle"></p><br></br>
                            <a>Date</a><br></br>
                            <input className="form-control" type="date" value={date} onChange = {(e) => {setDate(e.target.value); }}/><br></br>
                            <a>Type</a><br></br>
                            <input className="form-control" value={type} onChange = {(e) => {setType(e.target.value); }}/><br></br>
                            <a>Result</a><br></br>
                            <input className="form-control" value={result} onChange = {(e) => {setResult(e.target.value); }}/><br></br>
                            <a>Isolation</a><br></br>
                            <input className="form-control" value={isolation} onChange = {(e) => {setIsolation(e.target.value); }}/><br></br>
                        </div>
                        <div className="modal-footer">
                            <button type="button" className="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                            <button type="button" className="btn floatButton" onClick={() => saveEdit()}>Save changes</button>
                        </div>
                    </div>
                </div>
            </div>
            <div className="modal fade" id="createModal" tabIndex="-1" role="dialog" aria-labelledby="createModalLabel" aria-hidden="true">
                <div className="modal-dialog" role="document">
                    <div className="modal-content">
                        <div className="modal-header">
                            <h5 className="modal-title" id="createModalLabel">Create new Test</h5>
                            <button type="button" className="btn floatButton" data-bs-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div className="modal-body">
                        <p id="errorCreate" className="errorTextTitle"></p><br></br>
                            <a>Date</a><br></br>
                            <input className="form-control" type="date" value={date} onChange = {(e) => {setDate(e.target.value); }}/><br></br>
                            <a>Type</a><br></br>
                            <input className="form-control" value={type} onChange = {(e) => {setType(e.target.value); }}/><br></br>
                            <a>Result</a><br></br>
                            <input className="form-control" value={result} onChange = {(e) => {setResult(e.target.value); }}/><br></br>
                            <a>Isolation</a><br></br>
                            <input className="form-control" value={isolation} onChange = {(e) => {setIsolation(e.target.value); }}/><br></br>
                        </div>
                        <div className="modal-footer">
                            <button type="button" className="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                            <button type="button" className="btn floatButton" onClick={() => create()}>Create</button>
                        </div>
                    </div>
                </div>
            </div>
            <div className="modal fade" id="deleteModal" tabIndex="-1" role="dialog" aria-labelledby="deleteModalLabel" aria-hidden="true">
                <div className="modal-dialog" role="document">
                    <div className="modal-content">
                        <div className="modal-header">
                            <h5 className="modal-title" id="deleteModalLabel">Delete the Test</h5>
                            <button type="button" className="btn floatButton" data-bs-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div className="modal-body">
                            <p id="errorDelete" className="errorTextTitle"></p><br></br>
                            <a>Are you sure you want to delete the test?</a><br></br>
                        </div>
                        <div className="modal-footer">
                            <button type="button" className="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                            <button type="button" className="btn floatButton" onClick={() => DeleteTest()}>Delete</button>
                        </div>
                    </div>
                </div>
            </div>
            <footer className="footerLogin">
                <p>Copyright © 2022</p>
            </footer>
        </>
    )
}




