import React, { useState, useEffect } from 'react';
import Grid from '@mui/material/Grid';
import Link from '@mui/material/Link';
import Card from '@mui/material/Card';
import Typography from '@mui/material/Typography';
import CardHeader from '@mui/material/CardHeader';
import Button from '@mui/material/Button';
import Modal from '@mui/material/Modal';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Paper from '@mui/material/Paper';
import IconButton from '@mui/material/IconButton';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import FormLayoutsBasic from 'src/views/form-layouts/FormLayoutsBasic'; // FormLayoutsBasic bileşenini kullanacağız

const TransactionPage = () => {
  const [transactions, setTransactions] = useState([]);
  const [isFormOpen, setIsFormOpen] = useState(false);
  const [selectedTransaction, setSelectedTransaction] = useState(null);

  // API URL
  const apiUrl = 'http://localhost:5000/api/transaction';

  // Fetch transactions on component mount
  useEffect(() => {
    fetchTransactions();
  }, []);

  const fetchTransactions = async () => {
    try {
      const response = await fetch(apiUrl);
      if (response.ok) {
        const data = await response.json();
        setTransactions(data);
      } else {
        console.error('Error fetching transactions:', response.statusText);
      }
    } catch (error) {
      console.error('Error fetching transactions:', error.message);
    }
  };

  const openForm = (transaction) => {
    setSelectedTransaction(transaction);
    setIsFormOpen(true);
  };

  const closeForm = () => {
    setSelectedTransaction(null);
    setIsFormOpen(false);
    fetchTransactions(); // Refresh transaction list after form close
  };

  const handleDelete = async (id) => {
    try {
      const response = await fetch(`${apiUrl}/${id}`, {
        method: 'DELETE',
      });
      if (response.ok) {
        console.log('Transaction deleted successfully');
        fetchTransactions(); // Refresh transaction list after deletion
      } else {
        console.error('Failed to delete transaction:', response.statusText);
      }
    } catch (error) {
      console.error('Error deleting transaction:', error.message);
    }
  };

  return (
    <Grid container spacing={3}>
      <Grid item xs={12}>
        <Typography variant='h5'>
          <Link href='#'>İşlemler</Link>
        </Typography>
        <Typography variant='body2'>Tüm işlemlerin listesi</Typography>
        <Button variant='contained' color='primary' onClick={() => setIsFormOpen(true)}>
          Yeni İşlem Ekle
        </Button>
      </Grid>
      <Grid item xs={12}>
        <Card>
          <CardHeader title='İşlem Listesi' titleTypographyProps={{ variant: 'h6' }} />
          <TableContainer component={Paper}>
            <Table>
              <TableHead>
                <TableRow>
                  <TableCell>ID</TableCell>
                  <TableCell>Açıklama</TableCell>
                  <TableCell>Tutar</TableCell>
                  <TableCell>Tarih</TableCell>
                  <TableCell>İşlem Türü</TableCell>
                  <TableCell>İşlemler</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {transactions.map((transaction) => (
                  <TableRow key={transaction.transactionID}>
                    <TableCell>{transaction.transactionID}</TableCell>
                    <TableCell>{transaction.description}</TableCell>
                    <TableCell>{transaction.amount}</TableCell>
                    <TableCell>{new Date(transaction.transactionDate).toLocaleDateString()}</TableCell>
                    <TableCell>{transaction.transactionType}</TableCell>
                    <TableCell>
                      <IconButton color='primary' onClick={() => openForm(transaction)}>
                        <EditIcon />
                      </IconButton>
                      <IconButton color='secondary' onClick={() => handleDelete(transaction.transactionID)}>
                        <DeleteIcon />
                      </IconButton>
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>
        </Card>
      </Grid>
      <Modal open={isFormOpen} onClose={() => setIsFormOpen(false)}>
        <FormLayoutsBasic transaction={selectedTransaction} onSubmit={closeForm} />
      </Modal>
    </Grid>
  );
};

export default TransactionPage;
