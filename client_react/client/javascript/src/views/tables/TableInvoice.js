import React, { useState, useEffect } from 'react';
import Box from '@mui/material/Box';
import Paper from '@mui/material/Paper';
import Table from '@mui/material/Table';
import TableRow from '@mui/material/TableRow';
import TableHead from '@mui/material/TableHead';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import Button from '@mui/material/Button';
import TableContainer from '@mui/material/TableContainer';
import CheckIcon from '@mui/icons-material/Check';
import ClearIcon from '@mui/icons-material/Clear';
import { getInvoices,doPayment } from '../../service/Invoices';

const InvoicePage = () => {
  const [invoices, setInvoices] = useState([]);

  useEffect(() => {
    const fetchInvoices = async () => {
      try {
        const userId = localStorage.getItem('userId');
        const response = await getInvoices(userId);
        setInvoices(response);
      } catch (error) {
        console.error('Error fetching invoices:', error);
      }
    };

    fetchInvoices();
  }, []);

  const handlePay = async (invoiceID) => {
    try {
      console.log(`Paying invoice with ID: ${invoiceID}`);
      const response = await doPayment(invoiceID);
      setInvoices(response);
      // Call your API to handle the payment logic here
      // await payInvoice(invoiceID);
    } catch (error) {
      console.error('Error paying invoice:', error);
    }
  };

  return (
    <Box>
      <TableContainer component={Paper}>
        <Table aria-label='invoice history table'>
          <TableHead>
            <TableRow>
              <TableCell>Invoice ID</TableCell>
              <TableCell>Sender Account ID</TableCell>
              <TableCell>Receiver Account ID</TableCell>
              <TableCell>Date</TableCell>
              <TableCell>Amount</TableCell>
              <TableCell>Transaction Type</TableCell>
              <TableCell>Description</TableCell>
              <TableCell>Invoice Date</TableCell>
              <TableCell>Due Date</TableCell>
              <TableCell>Paid</TableCell>
              <TableCell>Action</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {invoices.length > 0 ? (
              invoices.map(invoice => (
                <TableRow key={invoice.invoiceID}>
                  <TableCell>{invoice.invoiceID}</TableCell>
                  <TableCell>{invoice.senderAccountID}</TableCell>
                  <TableCell>{invoice.receiverAccountID}</TableCell>
                  <TableCell>{invoice.date}</TableCell>
                  <TableCell>{invoice.amount}</TableCell>
                  <TableCell>{invoice.transactionType}</TableCell>
                  <TableCell>{invoice.description}</TableCell>
                  <TableCell>{invoice.invoiceDate}</TableCell>
                  <TableCell>{invoice.dueDate}</TableCell>
                  <TableCell>
                    {invoice.paid ? <CheckIcon /> : <ClearIcon />}
                  </TableCell>
                  <TableCell>
                    <Button 
                      variant="contained" 
                      color="primary" 
                      onClick={() => handlePay(invoice.invoiceID)}
                      disabled={invoice.paid}
                    >
                      Pay
                    </Button>
                  </TableCell>
                </TableRow>
              ))
            ) : (
              <TableRow>
                <TableCell colSpan={11}>No invoices available</TableCell>
              </TableRow>
            )}
          </TableBody>
        </Table>
      </TableContainer>
    </Box>
  );
}

export default InvoicePage;
