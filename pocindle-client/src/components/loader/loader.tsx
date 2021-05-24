import React from 'react';
import './loader.scss';

const Loader: React.FC = () => {
  return (
    <div className="loader">
      <div className="loader__ring"></div>
      <div className="loader__ring"></div>
      <div className="loader__ring"></div>
      <div className="loader__ring"></div>
    </div>
  );
};

export default Loader;
