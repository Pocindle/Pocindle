import React from 'react';
import AppRoute from './routes/appRoute';
import { ThemeProvider } from './providers';

const App: React.FC = () => {
  return (
    <React.Fragment>
      <ThemeProvider>
        <AppRoute />
      </ThemeProvider>
    </React.Fragment>
  );
};

export default App;
