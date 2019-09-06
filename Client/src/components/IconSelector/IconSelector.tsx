import React, { Component } from 'react';
import { Popover, Button, Row, Col, Icon } from 'antd';
import * as service from '../../services/service';

export default class IconSelector extends Component {
	state = {
		selectorVisible: false,
		iconClasses: []
	};

	componentDidMount() {
		let stylesMatch = /<[^>]+(?:src|href)=\s*["']?([^"]+\.(?:css))/g;
		let styleMatch = /<[^>]+(?:src|href)=\s*["']?([^"]+\.(?:css))/;
		let lines = document.documentElement.outerHTML.match(stylesMatch);
		let iconClasses = [];
		for (let i in lines) {
			var filename = lines[i].match(styleMatch)[1];
			service.getCssFile(filename).then((cssText) => {
				if (
					filename == 'index.css' &&
					(location.host.indexOf('localhost') >= 0 || location.host.indexOf('127.0.0.1') >= 0)
				) {
					cssText = document.documentElement.outerHTML;
				}
				let classesMatch = /\.anticon-([a-z\-]*):before/g;
				let classMatch = /\.anticon-([a-z\-]*)/;
				let cssLines = cssText.match(classesMatch);
				for (var i = 0; i < cssLines.length; i++) {
					var name = cssLines[i].match(classMatch)[1];
					if (name !== 'spin' && iconClasses.indexOf(name) < 0) {
						iconClasses.push(name);
					}
				}
			});
		}

		this.setState({
			iconClasses: iconClasses
		});
	}

	render() {
		const { selectorVisible, iconClasses } = this.state;
		let selectedIcon = this.props.value;
		const _this = this;
		function onIconSelected(name) {
			_this.setState({
				selectorVisible: false
			});
			_this.props.onChange(name);
		}

		function show() {
			_this.setState({
				selectorVisible: true
			});
		}

		function handleVisibleChange(visible) {
			_this.setState({
				selectorVisible: visible
			});
		}

		return (
			<Popover
				content={
					<div style={{ overflowY: 'auto', width: '420px', height: '320px' }}>
						<div style={{ borderBottom: '1px', width: '400px', height: '300px' }}>
							<Row>
								{iconClasses.map((name, index) => {
									return (
										<Col span={2} style={{ marginTop: 2 }} key={index}>
											<Button icon={name} onClick={onIconSelected.bind(this, name)} />
										</Col>
									);
								})}
							</Row>
						</div>
					</div>
				}
				title={<center>图标列表</center>}
				placement="bottomLeft"
				visible={selectorVisible}
				trigger="click"
				onVisibleChange={handleVisibleChange}
			>
				<Button onClick={show} icon={selectedIcon}>
					{selectedIcon && selectedIcon.length ? <span>{selectedIcon}</span> : <span>请选择图标</span>}
					<Icon type="down" />
				</Button>
			</Popover>
		);
	}
}
