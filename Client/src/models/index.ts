import appModel from './app';
import indexpage from './indexpage';
import home from './home';
import user from './user';
import auditLog from './auditLog';
import configuration from './configuration';
import menu from './menu';
import organization from './organization';
import role from './role';
import userLogin from './userLogin';
import register from './register';
import registerByEmail from './registerByEmail';
import emailregister from './emailregister';
import backknow from './backknow';
import callback from './callback';
import rentbyemail from './rentbyemail';
import notification from './notification';
import sendemail from './sendemail';
import confirmsucess from './confirmsucess';
import download from './download';
import thirdpartybinding from './thirdpartybinding';
import dragVerification from './dragVerification';
import geetest from './geetest';
import qrLogin from './qrLogin';
import getSet from './getSet';
import crud from './crud';
import serverImageBrowse from './serverImageBrowse';
import appEdition from './appEdition';
/**
 * index.js
 * Created by 李廷旭 on 2017/9/5 10:15
 * 描述: 注入models
 */
export function registerModels(app) {
	app.model(appModel);
	app.model(indexpage);
	app.model(home);
	app.model(user);
	app.model(auditLog);
	app.model(configuration);
	app.model(menu);
	app.model(organization);
	app.model(role);
	app.model(userLogin);
	app.model(register);
	app.model(registerByEmail);
	app.model(emailregister);
	app.model(backknow);
	app.model(callback);
	app.model(notification);
	app.model(rentbyemail);
	app.model(sendemail);
	app.model(confirmsucess);
	app.model(download);
	app.model(thirdpartybinding);
	app.model(dragVerification);
	app.model(geetest);
	app.model(qrLogin);
	app.model(getSet);
	app.model(crud);
	app.model(serverImageBrowse);
	app.model(appEdition);
}
