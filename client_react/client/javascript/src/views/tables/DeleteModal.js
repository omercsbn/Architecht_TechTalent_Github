import React, { useState } from 'react';
import Box from '@mui/material/Box';
import Modal from '@mui/material/Modal';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import { updateAccountName, deleteAccount, updateAccountBalance } from '../../service/Account'; // Assuming you have an API service for these operations
import TextField from '@mui/material/TextField';

const UpdateModal = ({ open, handleClose, account, onUpdate }) => {
  const [newAccountName, setNewAccountName] = useState(account.accountName);

  const handleUpdateName = async () => {
    try {
      await updateAccountName(account.accountID, newAccountName);
      onUpdate(); // Trigger parent component to refresh account information
      handleClose(); // Close modal
    } catch (error) {
      console.error('Error updating account name:', error);
    }
  };

  const [newBalance, setNewBalance] = useState(account.balance.toString());

  const handleUpdateBalance = async () => {
    try {
      await updateAccountBalance(account.accountID, parseFloat(newBalance));
      onUpdate(); // Trigger parent component to refresh account information
      handleClose(); // Close modal
    } catch (error) {
      console.error('Error updating account balance:', error);
    }
  };

  return (
    <Modal open={open} onClose={handleClose}>
      <Box
        sx={{
          position: 'absolute',
          top: '50%',
          left: '50%',
          transform: 'translate(-50%, -50%)',
          width: 400,
          bgcolor: 'background.paper',
          border: '2px solid #000',
          boxShadow: 24,
          p: 4,
          textAlign: 'center',
        }}
      >
        <Typography variant='h6' gutterBottom>
          Edit Account
        </Typography>
        <TextField
          label='New Account Name'
          variant='outlined'
          value={newAccountName}
          onChange={(e) => setNewAccountName(e.target.value)}
          fullWidth
          sx={{ mb: 2 }}
        />
        <TextField
          label='New Balance'
          variant='outlined'
          value={newBalance}
          onChange={(e) => setNewBalance(e.target.value)}
          type='number'
          step='0.01'
          fullWidth
          sx={{ mb: 2 }}
        />
        <Box sx={{ display: 'flex', justifyContent: 'center' }}>
          <Button variant='contained' onClick={handleUpdateName} sx={{ mr: 2 }}>
            Save Name
          </Button>
          <Button variant='contained' onClick={handleUpdateBalance}>
            Save Balance
          </Button>
        </Box>
      </Box>
    </Modal>
  );
};

const DeleteModal = ({ open, handleClose, account, onDelete }) => {
  const handleDelete = async () => {
    try {
      await deleteAccount(account.accountID);
      onDelete(); // Trigger parent component to delete account
      handleClose(); // Close modal
    } catch (error) {
      console.error('Error deleting account:', error);
    }
  };

  return (
    <Modal open={open} onClose={handleClose}>
      <Box
        sx={{
          position: 'absolute',
          top: '50%',
          left: '50%',
          transform: 'translate(-50%, -50%)',
          width: 400,
          bgcolor: 'background.paper',
          border: '2px solid #000',
          boxShadow: 24,
          p: 4,
        }}
      >
        <Typography variant='h6' gutterBottom>
          Delete Account
        </Typography>
        <Typography variant='body1' gutterBottom>
          {`Are you sure you want to delete the account "${account.accountName}" with ID "${account.accountID}"?`}
        </Typography>
        <Button variant='contained' onClick={handleDelete}>
          Delete
        </Button>
        <Button variant='outlined' onClick={handleClose} sx={{ ml: 2 }}>
          Cancel
        </Button>
      </Box>
    </Modal>
  );
};

export { UpdateModal, DeleteModal };
