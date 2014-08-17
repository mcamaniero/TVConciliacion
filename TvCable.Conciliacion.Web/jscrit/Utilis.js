/*! jQuery v1.6.4 http://jquery.com/ | http://jquery.org/license */
function alerta(numero) {
    alert('Número de registros afectados: ' + numero);
}

function Confirm() {
    var confirm_value = document.createElement("INPUT");
    confirm_value.type = "hidden";
    confirm_value.name = "confirm_value";
    if (confirm("¿Esta seguro que quiere actualizar el estado de la conciliación?")) {
        confirm_value.value = "Yes";
    } else {
        confirm_value.value = "No";
    }

    document.forms[0].appendChild(confirm_value);
}

function ConfirmIntermix() {
    var confirm_value = document.createElement("INPUT");
    confirm_value.type = "hidden";
    confirm_value.name = "confirm_valueitermix";
    if (confirm("¿Esta seguro que quiere enviar los datos a Intermix?")) {
        confirm_value.value = "Yes";
    } else {
        confirm_value.value = "No";
    }

    document.forms[0].appendChild(confirm_value);
}