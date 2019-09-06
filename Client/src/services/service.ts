import request from '../utils/request.js';
import fetch from 'dva/fetch';

export async function login2(options) {
	return request(`/Account/Login`, options);
}

export async function thirdPartyList(options) {
	return request(`/Account/ThirdPartyList`, options);
}

export async function thirdPartyLogin(options) {
	return request(`/Account/ThirdPartyLogin`, options);
}

export async function bindingThirdParty(options) {
	return request(`/Account/BindingThirdParty`, options);
}

export async function getBindingThirdPartyList(options) {
	return request(`/Account/GetBindingThirdPartyList`, options);
}

export async function loginUserBindingThirdParty(options) {
	return request(`/Account/LoginUserBindingThirdParty`, options);
}

export async function loginUserUnbindingThirdParty(options) {
	return request(`/Account/LoginUserUnbindingThirdParty`, options);
}

export async function qrlogin(options) {
	return request(`/QRLogin/Login`, options);
}

export async function logoutmy(options) {
	return request(`/Account/Logout`, options);
}

export async function getLessjs() {
	return fetch('/color/less.min.js', {
		method: 'get',
		withCredentials: true,
		credentials: 'include'
	}).then((response) => response.text());
}

export async function signalrjs() {
	return fetch(`${location.protocol}//${location.host}/signalr/hubs`, {
		method: 'get',
		withCredentials: true,
		credentials: 'include'
	}).then((response) => response.text());
}

export async function getServerImage(options) {
	return request(
		`/UE/controller.ashx?action=listimage&start=${options.param.start}&size=${options.param
			.size}&noCache=${Math.random()}`,
		options
	);
}
export async function getCssFile(path) {
	return fetch(path, {
		method: 'get',
		withCredentials: true,
		credentials: 'include'
	}).then((response) => response.text());
}
