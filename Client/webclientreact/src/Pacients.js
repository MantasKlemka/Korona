import React, { useEffect, useRef, useState} from "react";
import { Navigate, useNavigate } from "react-router-dom";
import { Button } from 'react-bootstrap';
import {ReactComponent as Logo} from './medication.svg';

export default function Pacients() {
    const navigate = useNavigate();
    const [tableRows, setTableRows] = useState([]);
    const [identification, setIdentification] = useState([]);
    const [name, setName] = useState([]);
    const [surname, setSurname] = useState([]);
    const [birthdate, setBirthdate] = useState([]);
    const [phone, setPhone] = useState([]);
    const [address, setAddress] = useState([]);
    const [doctor, setDoctor] = useState([]);
    const [pacientID, setPacientID] = useState([]);
    const [visibilityStatus, setVisibilityStatus] = useState([]);

    const [idEdit, setIdEdit] = useState([]);
    const [identificationEdit, setIdentificationEdit] = useState([]);
    const [nameEdit, setNameEdit] = useState([]);
    const [surnameEdit, setSurnameEdit] = useState([]);
    const [birthdateEdit, setBirthdateEdit] = useState([]);
    const [phoneEdit, setPhoneEdit] = useState([]);
    const [addressEdit, setAddressEdit] = useState([]);
    const [doctorEdit, setDoctorEdit] = useState([]);
    const [ownVisibleStatus, setOwnVisibleStatus] = useState([]);
    const [loading, setLoading] = useState([]);

    useEffect(() => {
        setLoading(false);
        setVisibilityStatus("hidden");
        if(sessionStorage.getItem("token") === null) {
            navigate("/");
        }
        if(sessionStorage.getItem("admin") === null){
            setOwnVisibleStatus("visible");
        }
        else{
            setOwnVisibleStatus("hidden");
        }
    }, []);

    function saveEdit(){
        setLoading(true);
        let json = {};
        if(identificationEdit != identification){
            json["identificationCode"] = identification;
        }
        if(nameEdit != name){
            json["name"] = name;
        }
        if(surnameEdit != surname){
            json["surname"] = surname;
        }
        if(birthdateEdit != birthdate){
            json["birthdate"] = birthdate;
        }
        if(phoneEdit != phone){
            json["phoneNumber"] = phone;
        }
        if(addressEdit != address){
            json["address"] = address;
        }
        if(doctorEdit != doctor){
            json["doctor"] = doctor;
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
        fetch('https://korona.azurewebsites.net/api/Pacient/', requestOptions)
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
        fetch('https://korona.azurewebsites.net/api/Pacient/' + idEdit, requestOptions)
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

    async function fetchGetOwn(){
        const requestOptions = {
            method: 'GET',
            headers: { 
                'Authorization':'Bearer ' + sessionStorage.getItem("token"),
                'Content-Type': 'application/json'}
        };
        const res = await fetch('https://korona.azurewebsites.net/api/Doctor/' + sessionStorage.getItem("doctorID") + '/Pacients', requestOptions)
        const data = await res.text();
        fillTable(res, data, "Own pacients");
    }

    async function fetchGetAll(){
        const requestOptions = {
            method: 'GET',
            headers: { 
                'Authorization':'Bearer ' + sessionStorage.getItem("token"),
                'Content-Type': 'application/json'}
        };
        const res = await fetch('https://korona.azurewebsites.net/api/Pacient/All/', requestOptions)
        const data = await res.text();
        fillTable(res, data, "All pacients");
    }

    async function fetchGetById(id){
        const requestOptions = {
            method: 'GET',
            headers: { 
                'Authorization':'Bearer ' + sessionStorage.getItem("token"),
                'Content-Type': 'application/json'}
        };
        const res = await fetch('https://korona.azurewebsites.net/api/Pacient/' + id, requestOptions);
        const data = await res.text();
        fillTable(res, data, "Pacient " + pacientID);
    }

    function fillTable(res, answer, title){
        if(res.status != 200)
        {
            let error = document.getElementById('errorLoad');
            error.textContent = answer;
        }
        else{
            document.getElementById('tableTitleText').textContent = title;
            var pacients = JSON.parse(answer);
            let i = 0;
            if(pacients.constructor == Array){
                pacients.map(pacient=>
                (
                    createRow(pacient, i),
                    i++
                ));
            }
            else{
                createRow(pacients, i)
            }
            setVisibilityStatus("visible");
        }
        setLoading(false);
    }

    function createRow(pacient, i){

        let identificationText = pacient.identificationCode;
        let nameText = pacient.name;
        let surnameText = pacient.surname;
        let birthDateText = pacient.birthDate.split("/");
        birthDateText = birthDateText[2] + "-" + (birthDateText[0] > 9 ? "" + birthDateText[0] : "0" + birthDateText[0]) + "-" + (birthDateText[1]> 9 ? "" + birthDateText[1] : "0" + birthDateText[1])
        let phoneText = pacient.phoneNumber;
        let addressText = pacient.address;
        let doctorText = pacient.doctor;

        let id = React.createElement('td',{key: "id" + i}, pacient.id);
        let identification = React.createElement('td',{key: "identification" + i}, identificationText);
        let name = React.createElement('td',{key: "name" + i}, nameText);
        let surname = React.createElement('td',{key: "surname" + i},surnameText);
        let birthdate = React.createElement('td',{key: "birthdate" + i}, birthDateText);
        let phone = React.createElement('td',{key: "phone" + i},phoneText);
        let address = React.createElement('td',{key: "address" + i},addressText);
        let doctor = React.createElement('td',{key: "doctor" + i},doctorText);
        let edit = React.createElement('td', {key: "edit" + i},"", React.createElement('button',{key: "editModal" + i, 'data-bs-toggle': "modal", 'data-bs-target': "#editModal", 'className': "btn floatButton", onClick: () => fillEdit(pacient.id, identificationText, nameText, surnameText, birthDateText, phoneText, addressText, doctorText) },"Edit"));
        let childs = [id, identification, name, surname, birthdate, phone, address, doctor, edit]
        let element = React.createElement('tr', {key: "row" + i}, childs);
        setTableRows(oldArray => [oldArray, element]);
    }


    function fillEdit(id, identification, name, surname, birthdate, phone, address, doctor){
        setIdentificationEdit(identification);
        setNameEdit(name);
        setSurnameEdit(surname);
        setBirthdateEdit(birthdate);
        setPhoneEdit(phone);
        setAddressEdit(address);
        setDoctorEdit(doctor);
        setIdEdit(id);
        setIdentification(identification);
        setName(name);
        setSurname(surname);
        setBirthdate(birthdate);
        setPhone(phone);
        setAddress(address);
        setDoctor(doctor);
    }

    function loadAll(){
        setLoading(true);
        setVisibilityStatus("hidden");
        document.getElementById('errorLoad').textContent = "";
        document.getElementById('tableTitleText').textContent = "";
        setTableRows();
        fetchGetAll();
    }

    function loadOwn(){
        setLoading(true);
        setVisibilityStatus("hidden");
        document.getElementById('errorLoad').textContent = "";
        document.getElementById('tableTitleText').textContent = "";
        setTableRows();
        fetchGetOwn();
    }

    function loadById(){
        setLoading(true);
        setVisibilityStatus("hidden");
        document.getElementById('errorLoad').textContent = "";
        document.getElementById('tableTitleText').textContent = "";
        setTableRows();
        fetchGetById(pacientID);
    }

    function create(){
        setLoading(true);
        if(identification.length === 0 || name.length === 0 || surname.length === 0 || birthdate.length === 0 || phone.length === 0 || address.length === 0 || doctor.length === 0){
            let error = document.getElementById('errorCreate');
            error.textContent = "All fields should be filled!"
        }
        else{
            let json = {};
            json["identificationCode"] = identification;
            json["name"] = name;
            json["surname"] = surname;
            json["birthdate"] = birthdate;
            json["phoneNumber"] = phone;
            json["address"] = address;
            json["doctor"] = doctor;
    
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
                        <a className="nav-link nav-link active">Pacients</a>
                        <a className="nav-item nav-link" href="/Isolations">Isolations</a>
                        <a className="nav-item nav-link" href="/Tests">Tests</a>
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
                    <input className="form-control inputs inputLoad" type="text" size="20" placeholder="Pacient ID" onChange = {(e) => {setPacientID(e.target.value); }}/>
                    <Button className="btn-secondary loadButton" onClick={() => loadById()}>Load Pacient by Id</Button><br></br><br></br>                
                    <Button className="btn-secondary loadButton" onClick={() => loadAll()}>Load All Pacients</Button><br></br>
                    <Button className="btn-secondary loadButton" style={{visibility: ownVisibleStatus}} onClick={() => loadOwn()}>Load Own Pacients</Button><br></br><br></br>
                    <Button className="btn-secondary loadButton" data-bs-toggle="modal" data-bs-target = "#createModal">Create new Pacient</Button>
                </div>
                
                <br></br><br></br><br></br><br></br>
                <b id="tableTitleText"></b>
                <table id="pacientsTable" className="table" style={{visibility: visibilityStatus}}>
                    <thead className="titleRow">
                        <tr>
                            <th scope="col">ID</th>
                            <th scope="col">Identification code</th>
                            <th scope="col">Name</th>
                            <th scope="col">Surname</th>
                            <th scope="col">Birth date</th>
                            <th scope="col">Phone number</th>
                            <th scope="col">Address</th>
                            <th scope="col">Doctor</th>
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
                            <a>Identification</a><br></br>
                            <input className="form-control" identification="identification" value={identification} onChange = {(e) => {setIdentification(e.target.value); }}/><br></br>
                            <a>Name</a><br></br>
                            <input className="form-control" identification="name"value={name} onChange = {(e) => {setName(e.target.value); }}/><br></br>
                            <a>Surname</a><br></br>
                            <input className="form-control" identification="surname"value={surname} onChange = {(e) => {setSurname(e.target.value); }}/><br></br>
                            <a>Birthdate</a><br></br>
                            <input className="form-control" type="date" identification="birthdate"value={birthdate} onChange = {(e) => {setBirthdate(e.target.value); }}/><br></br>
                            <a>Phone</a><br></br>
                            <input className="form-control" identification="phone"value={phone} onChange = {(e) => {setPhone(e.target.value); }}/><br></br>
                            <a>Address</a><br></br>
                            <input className="form-control" type="text" identification="address"value={address} onChange = {(e) => {setAddress(e.target.value); }}/><br></br>
                            <a>Doctor</a><br></br>
                            <input className="form-control" identification="doctor" value={doctor} onChange = {(e) => {setDoctor(e.target.value); }}/><br></br>
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
                            <h5 className="modal-title" id="createModalLabel">Create new Pacient</h5>
                            <button type="button" className="btn floatButton" data-bs-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div className="modal-body">
                        <p id="errorCreate" className="errorTextTitle"></p><br></br>
                            <a>Identification</a><br></br>
                            <input className="form-control" identification="identification" onChange = {(e) => {setIdentification(e.target.value); }}/><br></br>
                            <a>Name</a><br></br>
                            <input className="form-control" identification="name" onChange = {(e) => {setName(e.target.value); }}/><br></br>
                            <a>Surname</a><br></br>
                            <input className="form-control" identification="surname" onChange = {(e) => {setSurname(e.target.value); }}/><br></br>
                            <a>Birthdate</a><br></br>
                            <input className="form-control" type="date" identification="birthdate" onChange = {(e) => {setBirthdate(e.target.value); }}/><br></br>
                            <a>Phone</a><br></br>
                            <input className="form-control" identification="phone"onChange = {(e) => {setPhone(e.target.value); }}/><br></br>
                            <a>Address</a><br></br>
                            <input className="form-control" type="text" identification="address" onChange = {(e) => {setAddress(e.target.value); }}/><br></br>
                            <a>Doctor</a><br></br>
                            <input className="form-control" type="text" identification="doctor" onChange = {(e) => {setDoctor(e.target.value); }}/><br></br>
                        </div>
                        <div className="modal-footer">
                            <button type="button" className="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                            <button type="button" className="btn floatButton" onClick={() => create()}>Create</button>
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




