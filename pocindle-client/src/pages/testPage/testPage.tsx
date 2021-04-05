import React from 'react';
import MainLayout from '../../layouts/mainLayout/mainLayout';
import { TestComponent } from '../../components';

const TestPage: React.FC = () => {
  return (
    <React.Fragment>
      <MainLayout>
        Test page
        <TestComponent />
      </MainLayout>
    </React.Fragment>
  );
};

export default TestPage;
