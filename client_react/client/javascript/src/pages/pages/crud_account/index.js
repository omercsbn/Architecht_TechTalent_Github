import React, { useState, useEffect } from 'react';
import Grid from '@mui/material/Grid';
import Link from '@mui/material/Link';
import Card from '@mui/material/Card';
import Typography from '@mui/material/Typography';
import CardHeader from '@mui/material/CardHeader';
import Button from '@mui/material/Button';
import Modal from '@mui/material/Modal';
import TableCollapsible from 'src/views/tables/TableCollapsible';
import FormLayoutsBasic from 'src/views/form-layouts/FormLayoutsBasic'; // FormLayoutsBasic bileşenini kullanacağız

const AccountPage = () => {
  const [isFormOpen, setIsFormOpen] = useState(false);
  const [isNewAccountModalOpen, setIsNewAccountModalOpen] = useState(false);
  const [selectedAccount, setSelectedAccount] = useState(null);
  const [accounts, setAccounts] = useState([]);

  // API URL
  const apiUrl = 'http://localhost:5000/api/account';

  // Fetch accounts on component mount
  useEffect(() => {
    fetchAccounts();
  }, []);

  const fetchAccounts = async () => {
    try {
      const response = await fetch(`${apiUrl}/accounts`);
      if (response.ok) {
        const data = await response.json();
        setAccounts(data.data);
      } else {
        console.error('Error fetching accounts:', response.statusText);
      }
    } catch (error) {
      console.error('Error fetching accounts:', error.message);
    }
  };

  const openForm = (account) => {
    setSelectedAccount(account);
    setIsFormOpen(true);
  };

  const closeForm = () => {
    setSelectedAccount(null);
    setIsFormOpen(false);
  };

  const openNewAccountModal = () => {
    setIsNewAccountModalOpen(true);
  };

  const closeNewAccountModal = () => {
    setIsNewAccountModalOpen(false);
  };

  const handleCreateAccount = async (newAccount) => {
    try {
      const response = await fetch(`${apiUrl}/create`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(newAccount),
      });
      if (response.ok) {
        const data = await response.json();
        console.log('Account created successfully:', data);
        fetchAccounts(); // Refresh account list after creation
      } else {
        console.error('Failed to create account:', response.statusText);
      }
    } catch (error) {
      console.error('Error creating account:', error.message);
    }
  };

  const handleUpdateAccount = async (updatedAccount) => {
    try {
      const response = await fetch(`${apiUrl}/${updatedAccount.accountID}`, {
        method: 'PATCH',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(updatedAccount),
      });
      if (response.ok) {
        const data = await response.json();
        console.log('Account updated successfully:', data);
        fetchAccounts(); // Refresh account list after update
      } else {
        console.error('Failed to update account:', response.statusText);
      }
    } catch (error) {
      console.error('Error updating account:', error.message);
    }
  };

  const handleDeleteAccount = async (accountID) => {
    try {
      const response = await fetch(`${apiUrl}/${accountID}`, {
        method: 'DELETE',
      });
      if (response.ok) {
        const data = await response.json();
        console.log('Account deleted successfully:', data);
        fetchAccounts(); // Refresh account list after deletion
      } else {
        console.error('Failed to delete account:', response.statusText);
      }
    } catch (error) {
      console.error('Error deleting account:', error.message);
    }
  };

  return (
    <Grid container spacing={6}>
      <Grid item xs={12}>
        <Typography variant='h5'>
          <Link href='#' onClick={() => setIsFormOpen(false)}>Hesaplar</Link>
        </Typography>
        <Typography variant='body2'>Hesaplarınızın listesi</Typography>
        <Button variant='contained' color='primary' onClick={openNewAccountModal}>
          Yeni Hesap Oluştur
        </Button>
      </Grid>
      <Grid item xs={12}>
        <Card>
          <CardHeader title='Hesap Listesi' titleTypographyProps={{ variant: 'h6' }} />
          <TableCollapsible accounts={accounts} onEdit={openForm} onDelete={handleDeleteAccount} />
        </Card>
      </Grid>
      <Modal open={isFormOpen} onClose={closeForm}>
        <FormLayoutsBasic account={selectedAccount} onSubmit={handleUpdateAccount} onClose={closeForm} />
      </Modal>
      <Modal open={isNewAccountModalOpen} onClose={closeNewAccountModal}>
        <FormLayoutsBasic onSubmit={handleCreateAccount} onClose={closeNewAccountModal} />
      </Modal>
    </Grid>
  );
}

export default AccountPage;
