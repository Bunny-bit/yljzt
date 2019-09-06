export const remoteUrl =
	location.host.indexOf('localhost') >= 0 || location.host.indexOf('127.0.0.1') >= 0
		? `${location.protocol}//${location.host}/api`
		: `${location.protocol}//${location.host}`;
export const homePageUrl = '/echartall';
