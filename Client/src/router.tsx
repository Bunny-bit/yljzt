import React from 'react';
import { Router, Route } from 'dva/router';
import zhCN from 'antd/lib/locale-provider/zh_CN';
import { LocaleProvider } from 'antd';
import IndexPage from './routes/IndexPage';
import User from './routes/User/User';
import Home from './routes/Home/Home';
import AuditLog from './routes/AuditLog/AuditLog';
import Configuration from './routes/Configuration/Configuration';
import MenuList from './routes/Menu/MenuList';
import OrganizationList from './routes/Organization/OrganizationList';
import Register from './routes/Register/Register';
import RegisterByEmail from './routes/RegisterByEmail/RegisterByEmail';
import Emailregister from './routes/Emailregister/Emailregister';
import Activation from './routes/Activation/Activation';
import Echartall from './routes/Echartall/Echartall';
import Sendemail from './routes/Activation/Modal/Sendemail';
import Active from './routes/Activation/Modal/Active';
import Actsucess from './routes/Activation/Modal/Actsucess';
import Confirmsucess from './routes/Activation/Modal/Confirmsucess';
import Showregister from './routes/Emailregister/Modal/Showregister';
import Sucessregister from './routes/Emailregister/Modal/Sucessregister';
import Callsucess from './routes/Emailregister/Modal/Callsucess';
import Findpass from './routes/Findpass/Findpass';
import Backknow from './routes/Findpass/Modal/Backknow';
import Callback from './routes/Findpass/Modal/Callback';
import Rentbyemail from './routes/Findpass/Modal/Rentbyemail';
import Backsucess from './routes/Findpass/Modal/Backsucess';
import Role from './routes/Role/Role';
import Notification from './routes/Notification/Notification';
import Editor from './routes/Editor/Editor';
import ThirdPartyBinding from './routes/ThirdPartyBinding/ThirdPartyBinding';
import BindingLoginUser from './routes/ThirdPartyBinding/BindingLoginUser';
import QRLogin from './routes/Activation/Modal/QRLogin';
import AuditLogCRUDDemo from './routes/AuditLogCRUDDemo/AuditLog';
import Demo from './routes/Demo/Demo';
import GetsetDemo from './routes/GetsetDemo/GetsetDemo';
import UserCRUDDemo from './routes/UserCRUDDemo/User';
import AppEdition from './routes/AppEdition/AppEdition';
import AppStartPage from './routes/AppStartPage/AppStartPage';
import Luruyljzt from './routes/yljzt/Luruyljzt';
import Yljzt from './routes/yljzt/Yljzt';
import Xueyuan from './routes/Xueyuan/Xueyuan';
import LuruXueyuan from './routes/Xueyuan/LuruXueyuan';
import Renyua from './routes/Renyua/Renyua';
import Bingtu from './routes/Renyua/bingtu/bingtu';
import Renyuas from './routes/Renyua/Renyuas';
import Tankuang from './routes/Renyua/Tankuang';
import Renyuan from './routes/Renyuan/Renyuan';
import LuruRenyuan from './routes/Renyuan/LuruRenyuan';
import Datitu from './routes/Renyuan/Datitu';

/**
 * router.js
 * Created by 李廷旭 on 2017/9/5 12:39
 * 描述: 路由
 */
export default function RouterConfig({ history }) {
	return (
		<LocaleProvider locale={zhCN}>	
			<Router history={history}>
				<Route path="/" component={IndexPage} />
				<Route path="/register" component={Register} />
				<Route path="/registerByEmail" component={RegisterByEmail} />
				<Route path="/thirdpartybinding" component={ThirdPartyBinding} />
				<Route path="/binding" component={BindingLoginUser} />
				<Route path="/emailregister" component={Emailregister}>
					<Route path="/showres" component={Showregister} />
					<Route path="/sucess" component={Sucessregister} />
					<Route path="/callsucess" component={Callsucess} />
				</Route>
				<Route path="/" component={Activation}>
					<Route path="/sendemail" component={Sendemail} />
					<Route path="/active" component={Active} />
					<Route path="/actsucess" component={Actsucess} />
					<Route path="/confirm" component={Confirmsucess} />
					<Route path="/qrLogin" component={QRLogin} />
				</Route>
				<Route path="/" component={Findpass}>
					<Route path="/backknow" component={Backknow} />
					<Route path="/callback" component={Callback} />
					<Route path="/resetpassword" component={Rentbyemail} />
					<Route path="/backsucess" component={Backsucess} />
				</Route>
				<Route path="/" component={Home}>
					<Route path="/echartall" component={Echartall} />
					<Route path="/user" component={User} />
					<Route path="/role" component={Role} />
					<Route path="/auditLog" component={AuditLog} />
					<Route path="/configuration" component={Configuration} />
					<Route path="/menu" component={MenuList} />
					<Route path="/organization" component={OrganizationList} />
					<Route path="/notification" component={Notification} />
					<Route path="/editor" component={Editor} />
					<Route path="/auditLogCRUDDemo" component={AuditLogCRUDDemo} />
					<Route path="/demo" component={Demo} />
					<Route path="/getsetDemo" component={GetsetDemo} />
					<Route path="/userCRUDDemo" component={UserCRUDDemo} />
					<Route path="/appEdition" component={AppEdition} />
					<Route path="/appStartPage" component={AppStartPage} />
					<Route path="/yljzt" component={Yljzt} />
					<Route path="/xueyuan" component={Xueyuan} />
					<Route path="/renyuan" component={Renyua} />
					<Route path="/bingtu" component={Bingtu} />
					{/* <Route path="/renyuan" component={Renyuan} /> */}
					
				</Route>
			
				<Route path="/luruyljzt" component={Luruyljzt} />

				<Route path="/luruxueyuan" component={LuruXueyuan} />
				<Route path="/renyuas" component={Renyuas} />
				<Route path="/tankuang" component={Tankuang} />
			
			    <Route path="/lururenyuan" component={LuruRenyuan} />
				<Route path="/datitu" component={Datitu} />
			</Router>
		</LocaleProvider>
	);
}
