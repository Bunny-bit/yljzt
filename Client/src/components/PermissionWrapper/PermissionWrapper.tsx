import React, {Component} from 'react';
import {connect} from 'dva';

/**
 * PermissionWrapper.js
 * Created by 凡尧 on 2017/10/26 14:05
 * 描述: 权限
 */
class PermissionWrapper extends Component {
  constructor(props) {
    super(props);
  }
  isDisplay=(props)=>{
    if(props.requiredPermission && props.requiredPermission.length){
      if(this.props.permissions.indexOf(props.requiredPermission) < 0){
        return false;
      }
    }

    if(props.requiredAllPermissions && props.requiredAllPermissions.length){
      var permissions = props.requiredAllPermissions.split(',');
      for(let i = 0;i<permissions.length;i++){
        if(this.props.permissions.indexOf(permissions[i]) < 0){
          return false;
        }
      }
    }
    
    if(props.requiredAnyPermissions && props.requiredAnyPermissions.length){
      var permissions = props.requiredAnyPermissions.split(',');
      for(let i = 0;i<permissions.length;i++){
        if(this.props.permissions.indexOf(permissions[i]) >= 0){
          return true;
        }
      }
      return false;
    }
    return true;
  }

  render() {
    if(this.props.children && this.props.children.type && this.props.children.type.name ==="Menu"){
      var children=[];
      for(var i in this.props.children.props.children){
        if(!this.props.children.props.children[i] || !this.props.children.props.children[i].props){
          continue
        }
        if(this.isDisplay(this.props.children.props.children[i].props)){
          children.push( this.props.children.props.children[i]);
        }
      }
      return React.cloneElement(this.props.children, {children: children});
    }
    return (this.isDisplay(this.props)?this.props.children:null)
  }
}

export default connect((state) => {
  return {
    permissions:state.home.permissions,
  }
})(PermissionWrapper);
