import React from 'react';
import './authorizationLayout.scss';

const AuthorizationLayout: React.FC = ({ children }) => {
  return (
    <div className="authorization-layout">
      <div className="authorization-layout__wrapper">
        <div className="authorization-layout__col">{children}</div>
      </div>
    </div>
  );
};

export default AuthorizationLayout;
