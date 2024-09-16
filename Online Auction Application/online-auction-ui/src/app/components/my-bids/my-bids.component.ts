import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { BidService } from 'src/app/services/bid.service';

@Component({
  selector: 'app-my-bids',
  templateUrl: './my-bids.component.html',
  styleUrls: ['./my-bids.component.css']
})
export class MyBidsComponent implements OnInit {
  displayedColumns: string[] = ['productName', 'bidAmount', 'auctionEndTime', 'status'];
  dataSource: MatTableDataSource<any> = new MatTableDataSource<any>();
  constructor(private bidService: BidService) { }

  ngOnInit(): void {
    this.getMyBids();
  }

  getMyBids(): void {
    this.bidService.getBidsByUser().subscribe(
      (bids) => {
        this.dataSource.data = bids;
      },
      (error) => {
        console.error('Error fetching bids:', error);
      }
    );
  }
}
