import React, { Component } from 'react';
import './Ueditor/ueditor.config.js';
import './Ueditor/ueditor.all';
import './Ueditor/lang/zh-cn/zh-cn';
import './Ueditor.css';

/**
 * Ueditor.js
 * Created by 李廷旭 on 2017/8/11 11:05
 * 描述: Ueditor组件
 */
export default class Ueditor extends Component {
	constructor(props) {
		super(props);
		this.state = {};
	}

	componentDidMount() {
		this.initEditor();
	}

	componentWillUnmount() {
		// 组件卸载后，清除放入库的id
		UE.delEditor(this.props.id);
	}

	initEditor() {
		// console.log('initEditor', this.props.value);
		const id = this.props.id;
		// console.log(this.props);
		const ueEditor = UE.getEditor(id, {});
		const self = this;
		ueEditor.ready((ueditor) => {
			if (!ueditor) {
				UE.delEditor(id);
				self.initEditor();
			}
			if (this.props.value) {
				//初始化值
				UE.getEditor(this.props.id).setContent(this.props.value);
			}
			//添加时间监听，返回值
			ueEditor.addListener('contentChange', (e) => {
				this.props.onChange(UE.getEditor(id).getContent());
			});
		});
	}

	render() {
		// console.log('render', this.props.value);
		return <script id={this.props.id} type="text/plain" style={{ width: '100%', height: this.props.height }} />;
	}
}
