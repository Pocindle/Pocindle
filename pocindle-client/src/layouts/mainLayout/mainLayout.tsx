import React from 'react';
import { Navbar, Footer } from '../../components';
import './mainLayout.scss';
import useAppContext from '../../hooks/useAppContext';

const MainLayout: React.FC = ({ children }) => {
  const { language } = useAppContext();

  return (
    <div className="main-layout">
      <div className="main-layout__header">
        <Navbar
          articles={language.navbar.articles}
          logOut={language.navbar.logOut}
        />
      </div>
      <div className="main-layout__page-content">{children}</div>
      <div className="main-layout__footer">
        <Footer
          about={language.footer.about}
          siteTheme={language.footer.siteTheme}
          language={language.footer.language}
        />
      </div>
    </div>
  );
};

export default MainLayout;
