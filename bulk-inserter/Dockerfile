FROM alpine

WORKDIR /usr/bin/app

RUN apk add curl

## Copy wait script
COPY wait-for-200.sh /usr/bin/wait.sh
RUN chmod +x /usr/bin/wait.sh

COPY bulk.json bulk.json

## Add bulk.json data to ES
CMD wait.sh http://elastic-search:9200/_cat/health\?h\=st && \
    curl -X POST "elastic-search:9200/_bulk?pretty" \
    -H 'Content-Type: application/json' \
    --data-binary @bulk.json