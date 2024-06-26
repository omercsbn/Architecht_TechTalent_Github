import React, { useState, useEffect } from 'react';
import Box from '@mui/material/Box';
import Paper from '@mui/material/Paper';
import Table from '@mui/material/Table';
import TableRow from '@mui/material/TableRow';
import TableHead from '@mui/material/TableHead';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import Typography from '@mui/material/Typography';
import IconButton from '@mui/material/IconButton';
import TableContainer from '@mui/material/TableContainer';
import Modal from '@mui/material/Modal';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import MenuItem from '@mui/material/MenuItem';
import VisibilityIcon from '@mui/icons-material/Visibility';
import VisibilityOffIcon from '@mui/icons-material/VisibilityOff';
import EditIcon from '@mui/icons-material/Edit';

import { getAllTransactions, getFilteredTransactions, hideTransaction, updateTransaction } from '../../service/Transaction';

const Row = ({ transaction, handleHideTransaction, handleEditTransaction }) => {
  return (
    <TableRow sx={{ '& > *': { borderBottom: 'unset' } }}>
      <TableCell>{transaction.transactionID}</TableCell>
      <TableCell>{transaction.senderAccountID}</TableCell>
      <TableCell>{transaction.receiverAccountID}</TableCell>
      <TableCell>{transaction.amount}</TableCell>
      <TableCell>{transaction.transactionType}</TableCell>
      <TableCell>{transaction.description}</TableCell>
      <TableCell>{transaction.transactionDate}</TableCell>
      <TableCell>
        <IconButton onClick={() => handleHideTransaction(transaction.transactionID)}>
          {transaction.isHidden ? <VisibilityOffIcon /> : <VisibilityIcon />}
        </IconButton>
        <IconButton onClick={() => handleEditTransaction(transaction)}>
          <EditIcon />
        </IconButton>
      </TableCell>
    </TableRow>
  );
};

const TransactionHistory = () => {
  const [transactions, setTransactions] = useState([]);
  const [filterType, setFilterType] = useState('');
  const [filterValue, setFilterValue] = useState('');
  const [editTransaction, setEditTransaction] = useState(null);
  const [openEditModal, setOpenEditModal] = useState(false);
  const [validationErrors, setValidationErrors] = useState(null); // State to hold validation errors

  useEffect(() => {
    fetchTransactions();
  }, []);

  const fetchTransactions = async () => {
    try {
      const userId = localStorage.getItem('userId');
      const data = await getAllTransactions(userId);
      const visibleTransactions = data.filter(transaction => !transaction.isHidden);
      setTransactions(visibleTransactions);
    } catch (error) {
      console.error('Error fetching transactions:', error);
    }
  };
  
  const handleFilter = async () => {
    try {
      const userId = localStorage.getItem('userId');
      const data = await getFilteredTransactions(filterType, filterValue, userId);
      setTransactions(data);
    } catch (error) {
      console.error('Error filtering transactions:', error);
    }
  };

  const showAllTransactions = async () => {
    try {
      const userId = localStorage.getItem('userId');
      const data = await getAllTransactions(userId);
      setTransactions(data);
    } catch (error) {
      console.error('Error fetching all transactions:', error);
    }
  };

  const handleHideTransaction = async (transactionID) => {
    try {
      const updatedTransactions = transactions.map(transaction =>
        transaction.transactionID === transactionID ? { ...transaction, isHidden: !transaction.isHidden } : transaction
      );
      setTransactions(updatedTransactions);

      const userId = localStorage.getItem('userId');
      await hideTransaction(userId, transactionID);
    } catch (error) {
      console.error('Error updating transaction visibility:', error);
    }
  };

  const handleEditTransaction = (transaction) => {
    setEditTransaction(transaction);
    setOpenEditModal(true);
  };

  const handleSaveTransaction = async () => {
    try {
      const userId = localStorage.getItem('userId');
      await updateTransaction(userId, editTransaction);
      setOpenEditModal(false);
      fetchTransactions(); // Refresh transactions after edit
    } catch (error) {
      console.error('Error updating transaction:', error);
      if (error.response && error.response.status === 400) {
        setValidationErrors(error.response.data.errors); // Set validation errors from API response
      }
    }
  };

  return (
    <Box>
      <Box sx={{ mb: 2, display: 'flex', alignItems: 'center', marginLeft: '20px'}}>
        <TextField
          select
          label="Filter Type"
          value={filterType}
          onChange={(e) => setFilterType(e.target.value)}
          variant="outlined"
          sx={{ mr: 2 }}
        >
          <MenuItem value="senderAccountID">Sender Account ID</MenuItem>
          <MenuItem value="receiverAccountID">Receiver Account ID</MenuItem>
          <MenuItem value="transactionType">Transaction Type</MenuItem>
        </TextField>
        <TextField
          label="Filter Value"
          value={filterValue}
          onChange={(e) => setFilterValue(e.target.value)}
          variant="outlined"
          sx={{ mr: 2 }}
        />
        <Button variant="contained" onClick={handleFilter} sx={{ mr: 2 }}>Filter</Button>
        <Button variant="contained" onClick={showAllTransactions}>Show All</Button>
      </Box>
      <TableContainer component={Paper}>
        <Table aria-label='transaction history table'>
          <TableHead>
            <TableRow>
              <TableCell>Transaction ID</TableCell>
              <TableCell>Sender Account ID</TableCell>
              <TableCell>Receiver Account ID</TableCell>
              <TableCell>Amount</TableCell>
              <TableCell>Transaction Type</TableCell>
              <TableCell>Description</TableCell>
              <TableCell>Transaction Date</TableCell>
              <TableCell>Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {transactions.map(transaction => (
              <Row
                key={transaction.transactionID}
                transaction={transaction}
                handleHideTransaction={handleHideTransaction}
                handleEditTransaction={handleEditTransaction}
              />
            ))}
          </TableBody>
        </Table>
      </TableContainer>

      {/* Edit Transaction Modal */}
      <Modal
        open={openEditModal}
        onClose={() => setOpenEditModal(false)}
        aria-labelledby="edit-transaction-modal"
        aria-describedby="edit-transaction-details"
      >
        <Box sx={{
          position: 'absolute',
          top: '50%',
          left: '50%',
          transform: 'translate(-50%, -50%)',
          width: 400,
          bgcolor: 'background.paper',
          boxShadow: 24,
          p: 4,
        }}>
          <Typography variant="h6" gutterBottom>Edit Transaction</Typography>
          {editTransaction && (
            <Box>
              <TextField
                fullWidth
                label="Sender Account ID"
                value={editTransaction.senderAccountID}
                onChange={(e) => setEditTransaction({ ...editTransaction, senderAccountID: e.target.value })}
                variant="outlined"
                sx={{ mb: 2 }}
              />
              <TextField
                fullWidth
                label="Receiver Account ID"
                value={editTransaction.receiverAccountID}
                onChange={(e) => setEditTransaction({ ...editTransaction, receiverAccountID: e.target.value })}
                variant="outlined"
                sx={{ mb: 2 }}
              />
              <TextField
                fullWidth
                label="Amount"
                value={editTransaction.amount}
                onChange={(e) => setEditTransaction({ ...editTransaction, amount: e.target.value })}
                variant="outlined"
                sx={{ mb: 2 }}
              />
              <TextField
                fullWidth
                label="Transaction Type"
                value={editTransaction.transactionType}
                onChange={(e) => setEditTransaction({ ...editTransaction, transactionType: e.target.value })}
                variant="outlined"
                sx={{ mb: 2 }}
              />
              <TextField
                fullWidth
                label="Description"
                value={editTransaction.description}
                onChange={(e) => setEditTransaction({ ...editTransaction, description: e.target.value })}
                variant="outlined"
                sx={{ mb: 2 }}
              />
              <TextField
                fullWidth
                label="Transaction Date"
                value={editTransaction.transactionDate}
                onChange={(e) => setEditTransaction({ ...editTransaction, transactionDate: e.target.value })}
                variant="outlined"
                sx={{ mb: 2 }}
              />
              <TextField
                fullWidth
                label="Is Hide"
                value={editTransaction.isHidden}
                onChange={(e) => setEditTransaction({ ...editTransaction, isHidden: e.target.value })}
                variant="outlined"
                sx={{ mb: 2 }}
              />
              <TextField
                fullWidth
                label="User ID"
                value={editTransaction.userID}
                onChange={(e) => setEditTransaction({ ...editTransaction, userID: e.target.value })}
                variant="outlined"
                sx={{ mb: 2 }}
              />
              <Button variant="contained" onClick={handleSaveTransaction} sx={{ mr: 2 }}>Save</Button>
              <Button variant="contained" onClick={() => setOpenEditModal(false)}>Cancel</Button>
            </Box>
          )}
        </Box>
      </Modal>
    </Box>
  );
};

export default TransactionHistory;
