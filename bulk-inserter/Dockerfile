FROM curlimages/curl:7.73.0

WORKDIR /home/curl_user

## Copy wait script and bulk data
COPY wait-for-200.sh wait.sh
COPY bulk.json bulk.json

RUN chmod +x wait.sh

## Add bulk.json data to ES after the health endpoint returns HTTP 200
CMD ./wait.sh http://elastic-search:9200/_cat/health\?h\=st && \
    curl -X POST "elastic-search:9200/_bulk?pretty" \
    -H 'Content-Type: application/json' \
    --data-binary @bulk.json